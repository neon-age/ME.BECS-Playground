using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using Unity.Mathematics;
using UnityEngine;

public struct AI
{
    public struct Data : IConfigComponent
    {
        public float minDistToShoot;
    }
}

public struct AISystem : IUpdate
{
    public void OnUpdate(ref SystemContext ctx)
    {
        var playerEnt = PlayerInputsSystem.PlayerEnt;
        var playerTrs = playerEnt.GetAspect<TransformAspect>();
        var playerPos = playerTrs.readLocalPosition;

        var query = API.Query(ctx).WithAll<AI.Data, Character.Inputs>();

        foreach (var ent in query)
        {
            var aiTrs = ent.GetAspect<TransformAspect>();
            var aiPos = aiTrs.readLocalPosition;
            var aiData = ent.Read<AI.Data>();
            ref var charState = ref ent.Get<Character.State>();
            ref var charInput = ref ent.Get<Character.Inputs>();

            var dir = (playerPos - aiPos);
            var distToPlayer = math.length(dir);

            var isCloseToPlayer = distToPlayer < aiData.minDistToShoot;

            charInput.cursorPos = playerPos;

            charInput.move = math.normalize(new float2(dir.x, dir.z));
            charInput.shoot = isCloseToPlayer;

            if (!charState.weapon.IsEmpty())
            {
                // Don't decrease fire time when player is not in reach
                charState.weapon.Get<Weapon.State>().lockFireTime = !isCloseToPlayer;
            }
        }
    }
}