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

    public void OnDestroy(ref SystemContext context)
    {
        transformAccessArray.Dispose();
        registeredTransforms.Dispose();
        Debug.Log($"dispose");
    }

    void ValidateInitialization() // this will run before IAwake from other systems
    {
        if (!transformAccessArray.isCreated)
        {
            Debug.Log("validate");
            transformAccessArray = new TransformAccessArray(100);
            registeredTransforms = new UnsafeParallelHashMap<Ent, byte>(100, Allocator.Persistent);
        }
    }

    public static TransformAspect Register(Ent ent, Transform transform)
    {
        Debug.Log(Context.world.id);
        if (!Context.world.isCreated)
            return default;

        ref var system = ref Context.world.GetSystem<GOTransformSystem>();
        system.ValidateInitialization();

        //if (system.registeredTransforms.ContainsKey(ent))
        //    return default;

        var localPos = transform.localPosition;
        var localRot = transform.localRotation;
        var localScale = transform.localScale;

        var trsAspect = ent.GetAspect<TransformAspect>();
        trsAspect.localPosition = localPos;
        trsAspect.localRotation = localRot;
        trsAspect.localScale = localScale;

        system.transformAccessArray.Add(transform);
        system.registeredTransforms.Add(ent, 0);

        ref var goTrs = ref ent.Get<GOTransform>();

        goTrs.index = system.transformAccessArray.length - 1;
        goTrs.localPos = localPos;
        goTrs.localRot = localRot;
        goTrs.localScale = localScale;

        Debug.Log($"reg {transform} {trsAspect.localPosition} {ent}");
        return trsAspect;
    }
    public static void Unregister(Ent ent)
    {
        if (!Context.world.isCreated)
            return;
        Debug.Log($"del {ent}");
        ref var system = ref Context.world.GetSystem<GOTransformSystem>();
        if (!system.registeredTransforms.ContainsKey(ent))
            return;
        
        var go = ent.Read<GOTransform>();
        system.transformAccessArray.RemoveAtSwapBack(go.index);
    }

    public void OnUpdate(ref SystemContext context)
    {
        Debug.Log(Context.world.id);
        Debug.Log("init");
        var entities = new NativeList<Ent>(transformAccessArray.length, Allocator.TempJob);

        Debug.Log($"{transformAccessArray.length} {entities.Length}");

        var query = API.Query(context)
        .With<GOTransform>()
        .WithAspect<TransformAspect>();

        foreach (var ent in query)
        {
            entities.Add(ent);
            Debug.Log(ent);
        }
        //.Schedule(new CollectEntities() { entities = entities });

        var job = new ApplyTransformJob { entities = entities };

        var handle = job.Schedule(transformAccessArray);
        handle = entities.Dispose(handle);
        context.SetDependency(handle);
    }

    [BurstCompile]
    struct CollectEntities : IJobCommandBuffer
    {
        public NativeList<Ent> entities;

        public void Execute(in CommandBufferJob buffer)
        {
            entities.Add(buffer.ent);
        }
    }

    [BurstCompile]
    struct ApplyTransformJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeList<Ent> entities;
        
        public void Execute(int index, TransformAccess transform)
        {
            var ent = entities[index];

            var trsAspect = ent.GetAspect<TransformAspect>();
            ref var goRead = ref ent.Get<GOTransform>();

            var newLocalPos = trsAspect.localPosition;
            var newLocalRot = trsAspect.localRotation;
            var newLocalScale = trsAspect.localScale;
    
            var entityPosChanged = !goRead.localPos.Equals(newLocalPos);
            var entityRotChanged = !goRead.localRot.Equals(newLocalRot);
            var entityScaleChanged = !goRead.localScale.Equals(newLocalScale);

            
            {
                // sync transform to game-object entity
                if (entityPosChanged && entityRotChanged)
                {
                    transform.SetLocalPositionAndRotation(newLocalPos, newLocalRot);
                    Debug.Log($"to GO PosRot change {newLocalPos} {newLocalRot}");
                    goRead.localPos = newLocalPos;
                    goRead.localRot = newLocalRot;
                    //ent.Set(goRead); // increment version
                }
                else if (entityPosChanged)
                {
                    Debug.Log($"to GO Pos change {newLocalPos}");
                    transform.localPosition = newLocalPos;
                    goRead.localPos = newLocalPos; 
                    //ent.Set(goRead);
                }
                else if (entityRotChanged)
                {
                    //Debug.Log($"to GO Rot change {newLocalRot}");
                    transform.localRotation = newLocalRot;
                    goRead.localRot = newLocalRot;
                    //ent.Set(goRead);
                }
        
                if (entityScaleChanged)
                {
                    //Debug.Log($"to GO Scale change {newLocalScale}");
                    transform.localScale = newLocalScale;
                    goRead.localScale = newLocalScale;
                    //ent.Set(goRead);
                }
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
