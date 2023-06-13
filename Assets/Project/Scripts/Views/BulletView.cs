using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using UnityEngine;

public class BulletView : EntityView
{
    public EntityConfig config;
    public TrailRenderer trail;
    public GlobalGOHandle hitParticle;
    public GlobalGOHandle emitParticle;
    public View spawnSound;
    
    float startTrailTime;
    Vector3 startScale;

    protected override void OnInitialize(in Ent ent)
    {
        if (startTrailTime == 0)
        {
            startTrailTime = trail.time;
            startScale = transform.localScale;
        }
    }

    protected override void OnEnableFromPool(in Ent ent)
    {
        config.Apply(ent);

        var trs = ent.Transform();

        var pos = trs.localPosition;
        var rot = trs.localRotation;

        transform.localPosition = pos;
        transform.localScale = startScale;
        trs.localScale = startScale;

        trail.Clear();

        ref var state = ref ent.Get<Bullet.State>();
        state.trail = trail;
        state.isInitialized = true;

        ref var shared = ref ent.Get<Bullet.Shared>();
        shared.startTrailTime = startTrailTime;
        shared.startScale = startScale;
        shared.hitParticle = hitParticle;

        var sound = Ent.New();
        sound.Transform().LocalPosition(pos);
        sound.InstantiateView(spawnSound);

        var lifetime = ent.Read<LifetimeData>();
        state.emitParticle = ParticlesEmitterSystem.Emit(emitParticle.GetInstance<ParticleSystem>(), lifetime.value, pos, rot);
    }
/*
    protected override void OnDisableToPool()
    {
        trail.Clear();
    }*/
}
