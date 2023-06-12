using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using UnityEngine;

public struct Weapon
{
    [Serializable]
    public struct Data : IConfigComponent
    {
        public float fireRate;
        public View bulletView;
        [NonSerialized]
        public Ent firePoint;
        [NonSerialized]
        public ObjectReference<ParticleSystem> muzzleFlashParticle;
    }
    public struct State : IComponent
    {
        public Ent owner;
        public bool lockFireTime;
        public bool shootInput;
        public float fireTime;
    }
}

public struct WeaponSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var query = API.Query(ctx).WithAll<Weapon.Data, Weapon.State>();
        foreach (var ent in query)
        {
            ref var data = ref ent.Get<Weapon.Data>();
            ref var state = ref ent.Get<Weapon.State>();
            
            if (!state.lockFireTime)
                if (state.fireTime > 0)
                    state.fireTime -= ctx.deltaTime;

            if (state.fireTime <= 0 && state.shootInput)
            {
                state.fireTime = data.fireRate;

                data.firePoint.Transform().PositionRotation(out var firePos, out var fireRot);

                ParticlesEmitterSystem.Emit(data.muzzleFlashParticle.Value, 1, firePos, fireRot, data.firePoint);

                var bulletEnt = Ent.New();
                //bulletEnt.Get<BulletState>();
                bulletEnt.Get<Bullet.State>().ignoredEnt = state.owner;

                bulletEnt.Transform().LocalPositionRotation(firePos, fireRot);
                bulletEnt.InstantiateView(data.bulletView);
            }
        }
    }
}
