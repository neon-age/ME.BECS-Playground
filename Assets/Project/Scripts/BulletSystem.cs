using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using Unity.Mathematics;
using UnityEngine;

public struct BulletData : IComponent, IConfigComponent
{
    public float speed;
    public LayerMask rayMask;
}

public struct BulletSystem : IUpdate
{
    public void OnUpdate(ref SystemContext context)
    {
        API.Query(context).With<BulletData>().WithAspect<TransformAspect>().ForEach((in CommandBufferJob buffer) => 
        {
            ref var bullet = ref buffer.Get<BulletData>();
            var trs = buffer.ent.GetAspect<TransformAspect>();

            trs.localPosition += math.mul(trs.localRotation, new float3(0, 0, 1)) * bullet.speed;
            //Debug.Log(trs.readLocalPosition);
        });
    }
}
