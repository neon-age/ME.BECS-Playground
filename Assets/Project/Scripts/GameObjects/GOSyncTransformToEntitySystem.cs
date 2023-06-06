using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public struct GOSyncTransformToEntitySystem : IUpdate
{
    public void OnUpdate(ref SystemContext context)
    {
        foreach (var (trs, ent) in GOEntityLookup.TransformToEnt)
        {
            if (!ent.IsAlive() || !ent.Has<GOTransformSystem.TransformCache>())
                continue;

            // this could be very slow and buggy, need to find a better way to track changes
            if (trs.hasChanged)
            {
                trs.GetLocalPositionAndRotation(out var localPos, out var localRot);
                var localScale = trs.localScale;

                var trsAspect = ent.GetAspect<TransformAspect>();
                trsAspect.localPosition = localPos;
                trsAspect.localRotation = localRot;
                trsAspect.localScale = localScale;

                ref var trsCache = ref ent.Get<GOTransformSystem.TransformCache>();
                trsCache.localPos = localPos;
                trsCache.localRot = localRot;
                trsCache.localScale = localScale;
                /*
                ent.Get<LocalPositionComponent>().value = localPos;
                ent.Get<LocalRotationComponent>().value = localRot;
                ent.Get<LocalScaleComponent>().value = localScale;*/
            }
        }
    }
}