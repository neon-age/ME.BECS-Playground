using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

public struct LifetimeData : IComponent, IConfigComponent
{
    public float value;
}
public struct LifetimeSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var dt = ctx.deltaTime;
        var query = API.Query(ctx).With<LifetimeData>();

        foreach (var ent in query)
        {
            ref var lifetime = ref ent.Get<LifetimeData>();

            if (lifetime.value > 0)
                lifetime.value -= dt;
            else 
            {
                if (ent.Has<GOTransformSystem.TransformCache>())
                {
                    // Destroy game-object
                    GOTransformSystem.Unregister(ent);
                    if (GOEntityLookup.EntToTransform.TryGetValue(ent, out var trs))
                    {
                        Object.Destroy(trs.gameObject);
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
