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

public struct CharacterSystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var dt = ctx.deltaTime;

        var query = API.Query(ctx)
        .WithAll<CharacterData, CharacterInputsData>()
        .WithAspect<TransformAspect>();

        foreach (var ent in query)
        {
            var trs = ent.GetAspect<TransformAspect>();
            var state = ent.Read<CharacterState>();
            ref var data = ref ent.Get<CharacterData>();
            ref var input = ref ent.Get<CharacterInputsData>();
            var body = data.body.Value;

            var moveDir = new float3(input.move.x, 0, input.move.y) * (data.moveSpeed * dt);

            var vel = body.velocity;

            var velMoveDot = Vector3.Dot(vel, moveDir);
            var velMoveAngle = Vector3.Angle(vel, moveDir);
            
            if (velMoveDot != 0)
            {
                var dot = 0f;
                // Apply more force on sudden dir change
                //moveDir *= velDotMove < 0 ? data.moveDirChangeVel : 1;

                if (velMoveDot > 0)
                {
                    // Reduce velocity as it gets close to max velocity
                    dot = 1 - Mathf.Clamp01(velMoveDot / data.maxVelocity);
                    // Apply less velocity when approaching target angle
                    moveDir *= Mathf.Lerp(dot, 1, velMoveAngle / 180);
                }
                else // Apply more velocity when attempting to move in opposite direction 
                {
                    dot = (1 + Mathf.Clamp01(Mathf.Abs(velMoveDot) / data.maxVelocity)) * data.moveDirChangeVel;
                    moveDir *= dot;
                }
            }

            //if (!moveDir.Equals(default))
            //trs.localPosition += moveDir;
            if (!float.IsNaN(moveDir.x))
                body.AddForce(moveDir, ForceMode.VelocityChange);

            var localPos = trs.readLocalPosition;

            var weaponTrs = state.weapon.GetAspect<TransformAspect>();
            var cursorFromPos = weaponTrs.GetWorldMatrixPosition();

            cursorFromPos.y = localPos.y;
            //input.cursorPos.y = localPos.y;

            var lookDir = input.cursorPos - cursorFromPos;
            var lookRot = Quaternion.LookRotation(lookDir, Vector3.up);

            // Look towards with torque
            var angVel = body.angularVelocity;
            var torque = PhysicsUtil.GetTorqueTowardsRotation(angVel, trs.readLocalRotation, lookRot, data.lookSpring, data.lookDamper, dt);

            body.AddTorque(torque * dt, ForceMode.VelocityChange);

            // TODO: need trs.worldRotation to be able to do this...
            //weaponTrs.localRotation = Quaternion.LookRotation(input.cursorPos - cursorFromPos, Vector3.up);
            
            state.weapon.Get<WeaponState>().shootInput = input.shoot;
            
            input = default; // Reset input
        }
    }

   
}