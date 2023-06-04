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
        .WithAll<Character.Data, PlayerInputs.Data>()
        .WithAspect<TransformAspect>();
        //.ForEach((in CommandBufferJob buffer) => 
        foreach (var ent in query)
        {
            var trs = ent.GetAspect<TransformAspect>();
            ref var data = ref ent.Get<Character.Data>();
            ref var state = ref ent.Get<CharacterState>();
            ref var input = ref ent.Get<PlayerInputs.Data>();

            var moveDelta = data.moveSpeed * dt;
            var moveDir = new float3(input.move.x, 0, input.move.y) * moveDelta;

            if (!moveDir.Equals(default))
                trs.localPosition += moveDir;

            state.weapon.Get<WeaponState>().shootInput = input.shoot;
        }
    }
}