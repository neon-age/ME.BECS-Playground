using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using UnityEngine;

[Serializable]
public struct WeaponData : IComponent, IConfigComponent
{
    public float fireRate;
    public View bulletView;
    public Config bulletConfig;
    [NonSerialized]
    public Ent firePoint;
}
public struct WeaponState : IComponent
{
    public bool shootInput;
    public float fireTime;
}

public struct WeaponSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var query = API.Query(ctx).WithAll<WeaponData, WeaponState>();
        foreach (var ent in query)
        {
            ref var data = ref ent.Get<WeaponData>();
            ref var state = ref ent.Get<WeaponState>();
            
            if (state.fireTime > 0)
                state.fireTime -= ctx.deltaTime;

            if (state.fireTime <= 0 && state.shootInput)
            {
                state.fireTime = data.fireRate;

                var firePointTrs = data.firePoint.GetAspect<TransformAspect>();

                var bulletEnt = Ent.New();
                data.bulletConfig.Apply(bulletEnt);
                var bulletTrs = bulletEnt.GetAspect<TransformAspect>();

                bulletEnt.InstantiateView(data.bulletView);
                bulletTrs.localPosition = firePointTrs.GetWorldMatrixPosition();
                bulletTrs.localRotation = firePointTrs.GetWorldMatrixRotation();
            }
        }
    }
}
