using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

public struct HealthData : IConfigComponent
{
    public int value;
}

public struct HealthSystem : IUpdate
{
    public void OnUpdate(ref SystemContext context)
    {
        foreach (var ent in API.Query(context).With<HealthData>())
        {
            var health = ent.Read<HealthData>();

            if (health.value <= 0)
            {
                ent.Get<LifetimeData>().value = 0;
            }
        }
    }
}
