using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using UnityEngine;

public struct KillZoneData : IConfigComponent
{
    public LayerMask layerMask;
}

public struct KillZoneSystem : IUpdate
{
    static Collider[] overlaps = new Collider[10];

    public void OnUpdate(ref SystemContext ctx)
    {
        var query = API.Query(ctx).With<KillZoneData>();

        foreach (var ent in query)
        {
            var killZone = ent.Get<KillZoneData>();
            var trs = ent.GetAspect<TransformAspect>();
            var pos = trs.readLocalPosition;
            var rot = trs.readLocalRotation;
            var scale = trs.readLocalScale;
            var overlapCount = Physics.OverlapBoxNonAlloc(pos, scale, overlaps, rot, killZone.layerMask);

            for (int i = 0; i < overlapCount; i++)
            {
                var overlapEnt = overlaps[i].GetEntity();
                if (overlapEnt.IsAlive())
                {
                    var entToDestroy = overlapEnt;
                    if (overlapEnt.Has<HitboxLinkData>())
                    {
                        entToDestroy = overlapEnt.Get<HitboxLinkData>().linkedEntity;
                    }
                    entToDestroy.Get<LifetimeData>().value = 0;
                }
            }
        }
        //Physics.OverlapBoxNonAlloc
    }
}
