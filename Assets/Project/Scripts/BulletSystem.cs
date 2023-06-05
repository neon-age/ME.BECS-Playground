using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using Unity.Mathematics;
using UnityEngine;

public struct BulletData : IComponent, IConfigComponent
{
    public float speed;
    public LayerMask rayMask;
}

public struct BulletSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var dt = ctx.deltaTime;
        API.Query(ctx).With<BulletData>().WithAspect<TransformAspect>().ForEach((in CommandBufferJob buffer) => 
        {
            var ent = buffer.ent;
            ref var bullet = ref ent.Get<BulletData>();
            var trs = ent.GetAspect<TransformAspect>();

            trs.localPosition += math.mul(trs.readLocalRotation, new float3(0, 0, 1)) * bullet.speed;
        });
    }
}
