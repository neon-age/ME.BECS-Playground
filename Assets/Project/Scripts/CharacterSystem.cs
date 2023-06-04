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
        .WithAll<CharacterData, PlayerInputs.Data>()
        .WithAspect<TransformAspect>();
        foreach (var ent in query)
        {
            var trs = ent.GetAspect<TransformAspect>();
            ref var data = ref ent.Get<CharacterData>();
            ref var state = ref ent.Get<CharacterState>();
            ref var input = ref ent.Get<PlayerInputs.Data>();

            var moveDelta = data.moveSpeed * dt;
            var moveDir = new float3(input.move.x, 0, input.move.y) * moveDelta;

            //if (!moveDir.Equals(default))
            trs.localPosition += moveDir;

            var localPos = trs.readLocalPosition;
            input.cursorPos.y = localPos.y;

            trs.localRotation = Quaternion.LookRotation(input.cursorPos - localPos, Vector3.up);

            state.weapon.Get<WeaponState>().shootInput = input.shoot;
        }
    }
}