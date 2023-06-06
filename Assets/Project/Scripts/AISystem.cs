using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using Unity.Mathematics;
using UnityEngine;

public struct AIData : IComponent, IConfigComponent
{
    public float minDistToShoot;
}

public struct AISystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var query = API.Query(ctx)
        .WithAll<AIData, CharacterInputsData>()
        .WithAspect<TransformAspect>();

        var playerEnt = PlayerInputsSystem.PlayerEnt;
        var playerTrs = playerEnt.GetAspect<TransformAspect>();
        var playerPos = playerTrs.readLocalPosition;

        foreach (var ent in query)
        {
            var aiTrs = ent.GetAspect<TransformAspect>();
            var aiPos = aiTrs.readLocalPosition;
            var aiData = ent.Read<AIData>();
            ref var charState = ref ent.Get<CharacterState>();
            ref var charInput = ref ent.Get<CharacterInputsData>();

            var dir = (playerPos - aiPos);
            var distToPlayer = math.length(dir);

            charInput.cursorPos = playerPos;
            charInput.move = math.normalize(new float2(dir.x, dir.z));
            charInput.shoot = distToPlayer < aiData.minDistToShoot;
        }
    }
}