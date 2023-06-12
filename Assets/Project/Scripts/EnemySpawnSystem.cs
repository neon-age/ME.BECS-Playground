using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Views;
using UnityEngine;

public struct EnemySpawnSystem : IUpdate
{
    public View enemyPrefab;
    public float spawnRate;

    float spawnTime;

    public void OnUpdate(ref SystemContext context)
    {
        spawnTime -= context.deltaTime;
        if (spawnTime > 0)
            return;
        spawnTime = spawnRate;

        const float Offset = 0.2f; 
        float randomValue = Random.value;

        var randomSide = Random.Range(1, 4);

        Vector3 viewportPoint = default;
        switch (randomSide)
        {
            case 1: viewportPoint = new Vector3(-Offset, randomValue); break;
            case 2: viewportPoint = new Vector3(randomValue, 1 + Offset); break;
            case 3: viewportPoint = new Vector3(1 + Offset, randomValue); break;
            case 4: viewportPoint = new Vector3(randomValue, -Offset); break;
        }

        var enemyPrefabGO = ViewsRegistry.GetEntityViewByPrefabId(enemyPrefab.viewSource.prefabId).gameObject;

        var plane = new Plane(Vector3.up, Vector3.zero);
        var spawnRay = Camera.main.ViewportPointToRay(viewportPoint);
        plane.Raycast(spawnRay, out var rayDist);
        var spawnPoint = spawnRay.GetPoint(rayDist);

        Object.Instantiate(enemyPrefabGO, spawnPoint, Quaternion.identity);
    }
}
