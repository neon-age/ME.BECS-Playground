using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public struct LifetimeData : IConfigComponent
{
    public float value;
    [NonSerialized]
    public float startLifetime;
}

[Tooltip("Destroys entities and game-objects once their lifetime reached zero.")]
public struct LifetimeSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var dt = ctx.deltaTime;
        var query = API.Query(ctx).With<LifetimeData>();

        foreach (var ent in query)
        {
            ref var lifetime = ref ent.Get<LifetimeData>();

            if (lifetime.startLifetime == 0)
                lifetime.startLifetime = lifetime.value;

            if (lifetime.value > 0)
                lifetime.value -= dt;
            else 
            {
                // remove parent before destroy, otherwise we'll get error in TransformWorldMatrixUpdateSystem
                ent.SetParent(default);

                if (ent.Has<GOTransformSystem.TransformCache>())
                {
                    // Destroy game-object
                    GOTransformSystem.Unregister(ent);
                    if (GOEntityLookup.EntToTransform.TryGetValue(ent, out var trs))
                    {
                        UnityEngine.Object.Destroy(trs.gameObject);
                        // entity is removed in GOEntity.OnDestroy
                    }
                }
                else
                {
                    ent.Destroy();
                }
            }
        }
    }
}
