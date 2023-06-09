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
    float startTrailTime;
    Vector3 startScale;

    protected override void OnEnableFromPool(in Ent ent)
    {
        config.Apply(ent);

        transform.position = ent.GetAspect<TransformAspect>().position;

        ref var state = ref ent.Get<Bullet.State>();

        if (startTrailTime == 0)
        {
            startTrailTime = trail.time;
            startScale = transform.localScale;
        }

        trail.Clear();

        state.trail = trail;
        state.isInitialized = true;

        ref var shared = ref ent.GetShared<Bullet.Shared>();

        shared.startTrailTime = startTrailTime;
        shared.startScale = startScale;
    }
/*
    protected override void OnDisableToPool()
    {
        trail.Clear();
    }*/
}
