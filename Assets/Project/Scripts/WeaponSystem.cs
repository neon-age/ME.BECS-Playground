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
    public struct Data : IComponent, IConfigComponent
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

                var firePointTrs = data.firePoint.GetAspect<TransformAspect>();
                var firePos = firePointTrs.position;

                var muzzleParticle = ParticlesEmitterSystem.Emit(data.muzzleFlashParticle.Value);
                muzzleParticle.GetAspect<TransformAspect>().localPosition = firePos;

                var bulletEnt = Ent.New();
                //bulletEnt.Get<BulletState>();
                bulletEnt.Get<Bullet.State>().ignoredEnt = state.owner;
                
                var bulletTrs = bulletEnt.GetAspect<TransformAspect>(); 

                bulletTrs.localPosition = firePos;
                bulletTrs.localRotation = firePointTrs.rotation;

                bulletEnt.InstantiateView(data.bulletView);
            }
        }
    }
}
