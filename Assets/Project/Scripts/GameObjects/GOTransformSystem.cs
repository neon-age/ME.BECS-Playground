using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.Jobs;
using ME.BECS.TransformAspect;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public unsafe struct GOTransformSystem : IUpdate, IDestroy
{
    public struct GOIndex
    {
        public Ent ent;
        public int index;
    }
    public struct TransformCache : IComponent
    {
        /*
        [System.Flags]
        public enum SyncGO : byte
        {
            None = 0,
            Pos = 1 << 0,
            Rot = 1 << 1,
            Scale = 1 << 2,

            PosRot = Pos & Rot
        }
    */
        public float3 localPos;
        public quaternion localRot;
        public float3 localScale;
        //public SyncGO syncGO;
    }
    internal struct Command
    {
        public enum Type : byte
        {
            Register,
            Unregister,
        }
        public Type type;
        public Ent ent;
        public TransformCache trs;
    }

    TransformAccessArray transformAccessArray;
    UnsafeParallelHashMap<Ent, int> registeredTransforms;

    const int InitialCapacity = 100;

    struct PtrBuffer // can't use NativeList as it contains DisposeSentinel class, which marks system struct as managed
    {
        public UnsafeList<Command> commands;
        public UnsafeList<GOIndex> entities;

        public void Alloc()
        {
            commands = new UnsafeList<Command>(InitialCapacity, Allocator.Persistent);
            entities = new UnsafeList<GOIndex>(InitialCapacity, Allocator.Persistent);
        }
        public void Dispose()
        {
            commands.Dispose();
            entities.Dispose();
        }
    }
    PtrBuffer* buffer;
    
    int entityCount;
    bool isInitialized;

    public void OnDestroy(ref SystemContext context)
    {
        transformAccessArray.Dispose();
        registeredTransforms.Dispose();
       

        buffer->Dispose();
        Cuts._free(ref buffer);
        //UnsafeUtility.Free(buffer, Allocator.Persistent);
    }

    void ValidateInitialization() // can't use IAwake as it is called from game-objects before system initialization
    {
        if (!isInitialized)
        {
            isInitialized = true;
            transformAccessArray = new TransformAccessArray(InitialCapacity);
            registeredTransforms = new UnsafeParallelHashMap<Ent, int>(InitialCapacity, Allocator.Persistent);
           

            buffer = Cuts._make(new PtrBuffer());
            //buffer = (PtrBuffer*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<PtrBuffer>(), UnsafeUtility.AlignOf<PtrBuffer>(), Allocator.Persistent);
            buffer->Alloc();
        }
    }

    public static void Register(Ent ent, Transform transform)
    {
        if (!Context.world.isCreated)
            return;
        ref var system = ref Context.world.GetSystem<GOTransformSystem>();
        system._Register(ent, transform);
    }
    void _Register(Ent ent, Transform transform)
    {
        ValidateInitialization();
        if (registeredTransforms.ContainsKey(ent))
            return;

        var localPos = transform.localPosition;
        var localRot = transform.localRotation;
        var localScale = transform.localScale;

        var length = buffer->entities.Length;
        transformAccessArray.Add(transform);
        registeredTransforms.Add(ent, length);
        //Debug.Log($"reg {length}  {ent}");
        var trs = new TransformCache
        {
            localPos = localPos,
            localRot = localRot,
            localScale = localScale,
        };
        buffer->entities.Add(new GOIndex
        {
            ent = ent,
            index = length,
        });
        buffer->commands.Add(new Command { type = Command.Type.Register, ent = ent, trs = trs });

        entityCount++;
    }

    public static void Unregister(Ent ent)
    {
        if (!Context.world.isCreated)
            return;
        ref var system = ref Context.world.GetSystem<GOTransformSystem>();
        system._Unregister(ent);
    }
    void _Unregister(Ent ent)
    {
        if (!registeredTransforms.IsCreated || 
            !registeredTransforms.TryGetValue(ent, out var index))
            return;
        //Debug.Log($"{transformAccessArray.length} {buffer->entities.Length}");
        //Debug.Log($"del {index} {ent}");
        entityCount--;
        var lastEnt = buffer->entities[entityCount];
        transformAccessArray.RemoveAtSwapBack(index);
        buffer->entities.RemoveAtSwapBack(index);

        registeredTransforms[lastEnt.ent] = index;
        registeredTransforms.Remove(ent);

        //buffer->commands.Add(new Command { type = Command.Type.Unregister, ent = ent });
    }

    public void OnUpdate(ref SystemContext context)
    {
        var handle = new ExecuteRegisterCommandsJob { buffer = buffer }
        .Schedule();

        handle = new ApplyTransformJob { buffer = buffer }
        .Schedule(transformAccessArray, handle);

        context.SetDependency(handle);
    }
    [BurstCompile]
    struct ExecuteRegisterCommandsJob : IJob
    {
        [NativeDisableUnsafePtrRestriction]
        public PtrBuffer* buffer;

        public void Execute()
        {
            for (int i = buffer->commands.Length - 1; i >= 0; i--)
            {
                var c = buffer->commands[i];
                if (c.type == Command.Type.Register)
                {
                    var trs = c.ent.GetAspect<TransformAspect>();
                    trs.localPosition = c.trs.localPos;
                    trs.localRotation = c.trs.localRot;
                    trs.localScale = c.trs.localScale;

                    c.ent.Get<TransformCache>() = c.trs;
                }
                if (c.type == Command.Type.Unregister)
                {
                    c.ent.Remove<TransformCache>();
                }
                buffer->commands.RemoveAtSwapBack(i);
            }
        }
    }

    [BurstCompile]
    struct ApplyTransformJob : IJobParallelForTransform
    {
        [NativeDisableUnsafePtrRestriction]
        public PtrBuffer* buffer;
        
        public void Execute(int index, TransformAccess transform)
        {
            //if (index >= buffer->entities.Length || !transform.isValid)
            //    return;
            //Debug.Log(index);
            var ent = buffer->entities[index].ent;
            //if (!ent.IsAlive())
            //    return;
            var trsAspect = ent.GetAspect<TransformAspect>();
            var trsCache = ent.Read<TransformCache>();

            var newLocalPos = trsAspect.readLocalPosition;
            var newLocalRot = trsAspect.readLocalRotation;
            var newLocalScale = trsAspect.readLocalScale;
    
            var entityPosChanged = !trsCache.localPos.Equals(newLocalPos);
            var entityRotChanged = !trsCache.localRot.Equals(newLocalRot);
            var entityScaleChanged = !trsCache.localScale.Equals(newLocalScale);

            // sync transform from entity to game-object
            if (entityPosChanged && entityRotChanged)
            {
                transform.SetLocalPositionAndRotation(newLocalPos, newLocalRot);
                //Debug.Log($"to GO PosRot change {newLocalPos} {newLocalRot} {transform} {ent}");
                trsCache.localPos = newLocalPos;
                trsCache.localRot = newLocalRot;
                ent.Set(trsCache); // increment version
            }
            else if (entityPosChanged)
            {
                //Debug.Log($"to GO Pos change {newLocalPos} {transform} {ent}");
                transform.localPosition = newLocalPos;
                trsCache.localPos = newLocalPos;
                ent.Set(trsCache);
            }
            else if (entityRotChanged)
            {
                //Debug.Log($"to GO Rot change {newLocalRot}");
                transform.localRotation = newLocalRot;
                trsCache.localRot = newLocalRot;
                ent.Set(trsCache);
            }
        
            if (entityScaleChanged)
            {
                //Debug.Log($"to GO Scale change {newLocalScale}");
                transform.localScale = newLocalScale;
                trsCache.localScale = newLocalScale;
                ent.Set(trsCache);
            }


            /* // bad and clumsy idea, instead done via GOEntity parenting and GOSyncTransformToEntitySystem
            if (goRead.syncGO != 0)
            {
                // sync game-object transform to entity

                if (goRead.syncGO == GOTransform.SyncGO.PosRot)
                {
                    entityPosChanged = false;
                    entityRotChanged = false;
                    transform.GetLocalPositionAndRotation(out var localPos, out var localRot);
                    trsAspect.localPosition = localPos;
                    trsAspect.localRotation = localRot;
                    Debug.Log($"to Ent PosRot change {localPos} {localRot}");
                }
                else if (goRead.syncGO == GOTransform.SyncGO.Pos)
                {
                    entityPosChanged = false;
                    trsAspect.localPosition = transform.localPosition;
                    Debug.Log($"to Ent Pos change {trsAspect.localPosition}");
                }
                else if (goRead.syncGO == GOTransform.SyncGO.Rot)
                {
                    entityRotChanged = false;
                    trsAspect.localRotation = transform.localRotation;
                    Debug.Log($"to Ent Rot change {trsAspect.localRotation}");
                }

                if (goRead.syncGO == GOTransform.SyncGO.Scale)
                {
                    entityScaleChanged = false;
                    trsAspect.localScale = transform.localScale;
                    Debug.Log($"to Ent Scale change {trsAspect.localScale}");
                }
                
                //goRead.syncGO = 0;
                //ent.Set(goRead); // increment version
            }*/
        }
    }
}
