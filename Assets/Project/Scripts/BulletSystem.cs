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
    public float hitForce;
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
            var trs = ent.GetAspect<TransformAspect>();
            ref var data = ref ent.Get<BulletData>();
            ref var localPos = ref trs.localPosition;
            var localRot = trs.readLocalRotation;

            var prevPos = localPos;

            var dir = math.mul(localRot, new float3(0, 0, 1));

            localPos += dir * data.speed;

            if (Physics.Linecast(prevPos, localPos, out var hit, data.rayMask))
            {
                if (hit.collider.TryGetComponent(out Rigidbody hitBody))
                {
                    hitBody.AddForceAtPosition(dir * data.hitForce, hit.point, ForceMode.Impulse);
                }
                ent.Get<LifetimeData>().value = 0; // kill bullet
            }
        });
    }
}
