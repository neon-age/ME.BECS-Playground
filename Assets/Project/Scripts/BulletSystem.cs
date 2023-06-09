using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using Unity.Mathematics;
using UnityEngine;

public struct Bullet 
{
    public struct Shared : IConfigComponentShared
    {
        public float speed;
        public float hitForce;
        public LayerMask rayMask;

        [NonSerialized] public float startTrailTime;
        [NonSerialized] public Vector3 startScale;
    }
    public struct State : IComponent
    {
        public bool isDisabled;
        public bool isInitialized;
        public Ent ignoredEnt;
        public ObjectReference<TrailRenderer> trail;
    }
}

public struct BulletSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var dt = ctx.deltaTime;
        var query = API.Query(ctx).With<Bullet.State>();
        
        foreach (var ent in query)
        {
            ref var state = ref ent.Get<Bullet.State>();
            if (!state.isInitialized)
                continue;

            var trs = ent.GetAspect<TransformAspect>();
            var shared = ent.ReadShared<Bullet.Shared>();
            
            ref var lifetime = ref ent.Get<LifetimeData>();

            var lifetimeLerp = (lifetime.value / lifetime.startLifetime);

            var trail = state.trail.Value;
            var trailTime = shared.startTrailTime * lifetimeLerp;

            trail.time = trailTime;

            if (state.isDisabled)
            {
                trs.localScale = shared.startScale * lifetimeLerp;
                lifetime.value -= dt;
                continue;
            }

            ref var localPos = ref trs.localPosition;
            var localRot = trs.readLocalRotation;

            var prevPos = localPos;

            var dir = math.mul(localRot, new float3(0, 0, 1));

            localPos += dir * shared.speed;

            if (Physics.Linecast(prevPos, localPos, out var hit, shared.rayMask))
            {
                var hitCollider = hit.collider;
                var hitBody = hitCollider.attachedRigidbody;
                Ent hitEnt = hitBody ? hitBody.GetEntity() : hitCollider.GetEntity();

                if (hitEnt.Has<HitboxLinkData>())
                {
                    hitEnt = hitEnt.Get<HitboxLinkData>().linkedEntity;
                }

                if (hitEnt != state.ignoredEnt)
                {
                    if (hitBody)
                    {
                        hitBody.AddForceAtPosition(dir * shared.hitForce, hit.point, ForceMode.Impulse);
                    }
                    state.isDisabled = true;
                    lifetime.value = Mathf.Min(trailTime, shared.startTrailTime);
                }
            }
        }
    }
}
