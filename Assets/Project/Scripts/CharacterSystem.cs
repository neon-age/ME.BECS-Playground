using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.Jobs;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public struct Character
{
    public struct Inputs : IComponent
    {
        [NonSerialized] public float2 move;
        [NonSerialized] public float3 cursorPos;
        [NonSerialized] public bool shoot;
    }

    public struct State : IComponent
    {
        //[NonSerialized]
        public Ent weapon;
        [NonSerialized] public ObjectReference<Rigidbody> body;
        [NonSerialized] public GlobalGOHandle.ID deathParticle;
    }
    public struct Static : IConfigComponent
    {
        public float moveSpeed;
        public float groundDrag;
        //public float maxVelocity;
        public float lookSpring;
        public float lookDamper;
        public LayerMask groundMask;
        public float groundRadius;
        public float groundDist;
        public float groundUpforce;
    }
}

public struct CharacterSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var dt = ctx.deltaTime;

        var query = API.Query(ctx)
        .WithAll<Character.State, Character.Inputs>()
        .WithAspect<TransformAspect>();

        foreach (var ent in query)
        {
            var trs = ent.Transform()
            .LocalPositionRotation(out var localPos, out var localRot);

            var state = ent.Read<Character.State>();
            var shared = ent.Read<Character.Static>();
            ref var input = ref ent.Get<Character.Inputs>();

            var body = state.body.Value;
            var bodyVel = body.velocity;

            var trsUp = math.mul(localRot, new float3(0, 1, 0));

            var moveDir = new Vector3(input.move.x, 0, input.move.y) * (shared.moveSpeed * dt);

            bool isGrounded = Physics.SphereCast(localPos, shared.groundRadius, -trsUp, out var groundHit, shared.groundDist, shared.groundMask);


            if (ent.Has<LifetimeData>())
            {
                var lifetime = ent.Read<LifetimeData>();
                if (lifetime.value <= 0) // are we going to die in this frame?
                {
                    var deathParticle = state.deathParticle.GetInstance<ParticleSystem>();
                    ParticlesEmitterSystem.Emit(deathParticle, 10, localPos, Quaternion.identity);
                    continue;
                }
            }

            /* // feels bad lol, needs tweaking
            var velMoveDot = Vector3.Dot(bodyVel, moveDir);
            var velMoveAngle = Vector3.Angle(bodyVel, moveDir);
            if (velMoveDot != 0)
            {
                var dot = 0f;
                if (velMoveDot > 0)
                {
                    // Reduce velocity as it gets close to max velocity
                    dot = 1 - Mathf.Clamp01(velMoveDot / shared.maxVelocity);
                    // Apply less velocity when approaching target angle
                    moveDir *= Mathf.Lerp(dot, 1, velMoveAngle / 180);
                }
                else // Apply more velocity when attempting to move in opposite direction 
                {
                    dot = (1 + Mathf.Clamp01(Mathf.Abs(velMoveDot) / shared.maxVelocity));
                    moveDir *= dot;
                }
            }*/
            //Debug.Log(shared.groundDist);

            if (isGrounded) 
            {
                PhysicsUtil.ApplyDrag(ref bodyVel, 0, shared.groundDrag, dt);

                bodyVel += new Vector3(0, shared.groundUpforce * dt, 0);

                if (!float.IsNaN(moveDir.x))
                {
                    var groundNormalRot = Quaternion.FromToRotation(Vector3.up, groundHit.normal);

                    // Align move direction to ground normal
                    var moveForce = groundNormalRot * moveDir;

                    bodyVel += moveForce;
                }
            }

            //if (!moveDir.Equals(default))
            //trs.localPosition += moveDir;

            body.velocity = bodyVel;

            //input.cursorPos.y = localPos.y;

            var lookDir = input.cursorPos - localPos;
            var lookRot = Quaternion.LookRotation(lookDir, Vector3.up);

            // Look towards with torque
            var angVel = body.angularVelocity;
            var torque = PhysicsUtil.GetTorqueTowardsRotation(angVel, localRot, lookRot, shared.lookSpring, shared.lookDamper, dt);

            body.AddTorque(torque * dt, ForceMode.VelocityChange);

            if (state.weapon.IsAlive())
            {
                ref var weaponState = ref state.weapon.Get<Weapon.State>();

                weaponState.owner = ent;
                weaponState.shootInput = input.shoot;

                var weaponTrs = state.weapon.Transform().PositionRotation(out var pos, out var rot);

                // Rotate weapon X towards cursor
                var weaponLookAtRot = Quaternion.LookRotation(input.cursorPos - weaponTrs.position, trsUp);
                var localRotEuler = ((Quaternion)localRot).eulerAngles;
                weaponTrs.rotation = Quaternion.Euler(weaponLookAtRot.eulerAngles.x, localRotEuler.y, localRotEuler.z);
            }
            
            input = default; // Reset input
        }
    }

   
}