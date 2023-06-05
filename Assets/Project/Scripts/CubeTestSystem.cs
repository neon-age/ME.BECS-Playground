using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using Unity.Mathematics;
using UnityEngine;

public struct CubeTestSystem : IAwake, IUpdate
{
    public struct Cube : IComponent
    {
        public float speed;
        public float startLifetime;
    }
    public View cubeView; // as of now, ObjectReference<> doesn't support referencing prefabs, so use View instead
    public float spawnRadius;
    public int spawnCount;
    public float spawnRate;
    public float maxSpeed;
    public float maxLifetime;

    float spawnTime;

    EntityView cubePrefab => ViewsRegistry.GetEntityViewByPrefabId(cubeView.viewSource.prefabId);

    public void OnAwake(ref SystemContext context)
    {
        var prefab = cubePrefab;

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnCube(prefab);
        }
    }

    void SpawnCube(EntityView prefab)
    {
        var ent = Ent.New();
        var pos = UnityEngine.Random.insideUnitSphere * spawnRadius;
        var go = Object.Instantiate(prefab, pos, Quaternion.identity);

        GOTransformSystem.Register(ent, go.transform);
        GOEntityLookup.LinkEntity(ent, go.transform);

        var lifetime = UnityEngine.Random.value * maxLifetime;
        ent.Set(new Cube { speed = UnityEngine.Random.value * maxSpeed, startLifetime = lifetime });
        ent.Set(new LifetimeData { value = lifetime });
    }

    public void OnUpdate(ref SystemContext context)
    {
        if (spawnTime > 0)
            spawnTime -= context.deltaTime;
        else
            SpawnCube(cubePrefab);

        var query = API.Query(context).With<Cube>().WithAspect<TransformAspect>();

        foreach (var ent in query)
        {
            ref var cube = ref ent.Get<Cube>();
            var lifetime = ent.Read<LifetimeData>();
            var trs = ent.GetAspect<TransformAspect>();

            trs.localPosition += new float3(0, 0, cube.speed * context.deltaTime);
            trs.localScale = new float3(1, 1, 1) * (lifetime.value / cube.startLifetime);
        }
    }
}
