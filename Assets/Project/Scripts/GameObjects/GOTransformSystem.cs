using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public struct GOTransform : IComponent
{
    [System.Flags]
    public enum SyncGO : byte
    {
        None = 0,
        Pos = 1 << 0,
        Rot = 1 << 1,
        Scale = 1 << 2,

        PosRot = Pos & Rot
    }

    public int index;
    public float3 localPos;
    public quaternion localRot;
    public float3 localScale;
    public SyncGO syncGO;
}

[BurstCompile]
public struct GOTransformSystem : IUpdate, IDestroy
{
    TransformAccessArray transformAccessArray;

    public void OnDestroy(ref SystemContext context)
    {
        transformAccessArray.Dispose();
    }

    void ValidateTransformArray()
    {
        if (!transformAccessArray.isCreated)
            transformAccessArray = new TransformAccessArray(100);
    }

    public static void Register(Ent ent, Transform transform)
    {
        ref var system = ref Context.world.GetSystem<GOTransformSystem>();

        system.ValidateTransformArray();
        system.transformAccessArray.Add(transform);

        ent.Get<GOTransform>() = new GOTransform 
        { 
            index = system.transformAccessArray.length - 1,
            localPos = transform.localPosition,
            localRot = transform.localRotation,
            localScale = transform.localScale
        };
    }
    public static void Unregister(Ent ent)
    {
        ref var system = ref Context.world.GetSystem<GOTransformSystem>();
        var go = ent.Read<GOTransform>();
        system.transformAccessArray.RemoveAtSwapBack(go.index);
    }

    
    public void OnUpdate(ref SystemContext context)
    {
        var entities = new UnsafeList<Ent>(transformAccessArray.length, Allocator.TempJob);

        var getEntitiesHandle = API.Query(context)
        .With<GOTransform>()
        .WithAspect<TransformAspect>()
        .ForEach((in CommandBufferJob buffer) => 
        {
            entities.Add(buffer.ent);
        });
        var job = new ApplyTransformJob { entities = entities };
        
        var handle = job.Schedule(transformAccessArray, getEntitiesHandle);
        context.SetDependency(handle);
    }

    [BurstCompile]
    struct ApplyTransformJob : IJobParallelForTransform
    {
        [DeallocateOnJobCompletion]
        public UnsafeList<Ent> entities;
        
        public void Execute(int index, TransformAccess transform)
        {
            var ent = entities[index];

            var trsAspect = ent.GetAspect<TransformAspect>();
            var goRead = ent.Read<GOTransform>();

            var newLocalPos = trsAspect.readLocalPosition;
            var newLocalRot = trsAspect.readLocalRotation;
            var newLocalScale = trsAspect.readLocalScale;
    
            var entityPosChanged = !goRead.localPos.Equals(newLocalPos);
            var entityRotChanged = !goRead.localRot.Equals(newLocalRot);
            var entityScaleChanged = !goRead.localScale.Equals(newLocalScale);
            
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
            }
            //else 
            {
                // sync transform to game-object entity
                if (entityPosChanged && entityRotChanged)
                {
                    transform.SetLocalPositionAndRotation(newLocalPos, newLocalRot);
                    Debug.Log($"to GO PosRot change {newLocalPos} {newLocalRot}");
                    goRead.localPos = newLocalPos;
                    goRead.localRot = newLocalRot;
                    ent.Set(goRead); // increment version
                }
                else if (entityPosChanged)
                {
                    Debug.Log($"to GO Pos change {newLocalPos}");
                    transform.localPosition = newLocalPos;
                    goRead.localPos = newLocalPos; 
                    ent.Set(goRead);
                }
                else if (entityRotChanged)
                {
                    Debug.Log($"to GO Rot change {newLocalRot}");
                    transform.localRotation = newLocalRot;
                    goRead.localRot = newLocalRot;
                    ent.Set(goRead);
                }
        
                if (entityScaleChanged)
                {
                    Debug.Log($"to GO Scale change {newLocalScale}");
                    transform.localScale = newLocalScale;
                    goRead.localScale = newLocalScale;
                    ent.Set(goRead);
                }
            }
        }
    }
}
