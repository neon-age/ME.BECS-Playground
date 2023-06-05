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

public struct GOTransform : IComponent
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
    public int index;
    public float3 localPos;
    public quaternion localRot;
    public float3 localScale;
    //public SyncGO syncGO;
}

[BurstCompile]
public struct GOTransformSystem : IUpdate, IDestroy
{
    TransformAccessArray transformAccessArray;
    UnsafeParallelHashMap<Ent, byte> registeredTransforms;
    NativeArray<Ent> entities;
    int entityCount;

    public void OnDestroy(ref SystemContext context)
    {
        transformAccessArray.Dispose();
        registeredTransforms.Dispose();
        //entities.Dispose();
    }

    void ValidateInitialization() // can't use IAwake as it is called from game-objects before system initialization
    {
        if (!transformAccessArray.isCreated)
        {
            const int InitialCapacity = 100;
            transformAccessArray = new TransformAccessArray(InitialCapacity);
            registeredTransforms = new UnsafeParallelHashMap<Ent, byte>(InitialCapacity, Allocator.Persistent);
            entities = new NativeArray<Ent>(InitialCapacity, Allocator.Persistent);
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

        var trsAspect = ent.GetAspect<TransformAspect>();
        trsAspect.localPosition = localPos;
        trsAspect.localRotation = localRot;
        trsAspect.localScale = localScale;

        var length = entities.Length;
        transformAccessArray.Add(transform);
        registeredTransforms.Add(ent, 0);
        if (entities.Length <= length)
        {
            var prevArray = entities;
            entities = new NativeArray<Ent>(length + length / 2, Allocator.Persistent);
            for (int i = 0; i < length; i++)
            {
                entities[i] = prevArray[i];
            }
            prevArray.Dispose();
        }
        entities[entityCount] = ent;

        ref var goTrs = ref ent.Get<GOTransform>();

        goTrs.index = entityCount;
        goTrs.localPos = localPos;
        goTrs.localRot = localRot;
        goTrs.localScale = localScale;

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
        if (!registeredTransforms.IsCreated || !registeredTransforms.ContainsKey(ent))
            return;
        ref var go = ref ent.Get<GOTransform>();
        entityCount--;
        transformAccessArray.RemoveAtSwapBack(go.index);
        entities[go.index] = entities[entityCount];
        entities[entityCount] = default;

        go.index = entityCount;
    }

    public void OnUpdate(ref SystemContext context)
    {
        var job = new ApplyTransformJob { entities = entities };

        var handle = job.Schedule(transformAccessArray);
        context.SetDependency(handle);
    }

    [BurstCompile]
    struct ApplyTransformJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeArray<Ent> entities;
        
        public void Execute(int index, TransformAccess transform)
        {
            var ent = entities[index];

            var trsAspect = ent.GetAspect<TransformAspect>();
            var goCache = ent.Read<GOTransform>();

            var newLocalPos = trsAspect.localPosition;
            var newLocalRot = trsAspect.localRotation;
            var newLocalScale = trsAspect.localScale;
    
            var entityPosChanged = !goCache.localPos.Equals(newLocalPos);
            var entityRotChanged = !goCache.localRot.Equals(newLocalRot);
            var entityScaleChanged = !goCache.localScale.Equals(newLocalScale);

            // sync transform from entity to game-object
            if (entityPosChanged && entityRotChanged)
            {
                transform.SetLocalPositionAndRotation(newLocalPos, newLocalRot);
                //Debug.Log($"to GO PosRot change {newLocalPos} {newLocalRot} {transform} {ent}");
                goCache.localPos = newLocalPos;
                goCache.localRot = newLocalRot;
                ent.Set(goCache); // increment version
            }
            else if (entityPosChanged)
            {
                //Debug.Log($"to GO Pos change {newLocalPos} {transform} {ent}");
                transform.localPosition = newLocalPos;
                goCache.localPos = newLocalPos; 
                ent.Set(goCache);
            }
            else if (entityRotChanged)
            {
                //Debug.Log($"to GO Rot change {newLocalRot}");
                transform.localRotation = newLocalRot;
                goCache.localRot = newLocalRot;
                ent.Set(goCache);
            }
        
            if (entityScaleChanged)
            {
                //Debug.Log($"to GO Scale change {newLocalScale}");
                transform.localScale = newLocalScale;
                goCache.localScale = newLocalScale;
                ent.Set(goCache);
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
