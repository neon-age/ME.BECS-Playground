using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public struct GOSyncTransformToEntitySystem : IUpdate
{
    public void OnUpdate(ref SystemContext context)
    {
        foreach (var (trs, entKey) in GOEntityLookup.TransformToEnt)
        {
            var ent = new Ent(entKey);

            // this could be very slow and buggy, need to find a better way to track changes
            if (trs.hasChanged)
            {
                trs.GetLocalPositionAndRotation(out var localPos, out var localRot);
                var localScale = trs.localScale;

                ent.Get<LocalPositionComponent>().value = localPos;
                ent.Get<LocalRotationComponent>().value = localRot;
                ent.Get<LocalScaleComponent>().value = localScale;
            }
        }
    }
}