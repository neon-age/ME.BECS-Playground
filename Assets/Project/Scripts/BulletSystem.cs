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
    public struct Config : IConfigComponent
    {
        public float speed;
        public float radius;
        public float hitForce;
        public int damage;
        public LayerMask rayMask;
    }
    public struct Shared : IComponent
    {
        [NonSerialized] public float startTrailTime;
        [NonSerialized] public Vector3 startScale;
        [NonSerialized] public GlobalGOHandle.ID hitParticle;
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
            var state = ent.Read<Bullet.State>();
            if (!state.isInitialized)
                continue;

            var trs = ent.GetAspect<TransformAspect>();
            var config = ent.Read<Bullet.Config>();
            var shared = ent.Read<Bullet.Shared>();
            
            ref var lifetime = ref ent.Get<LifetimeData>();

            var lifetimeLerp = Mathf.Clamp01(lifetime.value / lifetime.startLifetime);

            var trail = state.trail.Value;
            var trailTime = shared.startTrailTime * lifetimeLerp;
            //Debug.Log(trs.localScale);

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
            var rayDir = prevPos - localPos;
            var rayDist = math.length(rayDir);

            var dir = math.mul(localRot, new float3(0, 0, 1));

            localPos += dir * config.speed;

            Debug.DrawRay(prevPos, math.normalize(rayDir) * 10, Color.red);

            if (Physics.Linecast(prevPos, localPos, out var hit, config.rayMask))
            {
                var hitCollider = hit.collider;
                var hitBody = hitCollider.attachedRigidbody;
                Ent hitEnt = hitBody ? hitBody.GetEntity() : hitCollider.GetEntity();

                if (hitEnt.IsAlive() && hitEnt.Has<HitboxLinkData>())
                {
                    hitEnt = hitEnt.Get<HitboxLinkData>().linkedEntity;
                }

                if (hitEnt.IsAlive() && hitEnt != state.ignoredEnt)
                {
                    var hitPoint = hit.point;
                    var hitRot = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                    if (hitEnt.Has<HealthData>())
                    {
                        hitEnt.Get<HealthData>().value -= config.damage;
                    }

                    if (hitBody)
                    {
                        hitBody.AddForceAtPosition(dir * config.hitForce, hit.point, ForceMode.Impulse);
                    }

                    var hitParticle = shared.hitParticle.GetInstance<ParticleSystem>();
                    ParticlesEmitterSystem.Emit(hitParticle, 1, hitPoint, hitRot);

                    lifetime.value = Mathf.Min(trailTime, shared.startTrailTime);

                    state.isDisabled = true;
                    ent.Set(state);
                }
            }
        }
    }
}
