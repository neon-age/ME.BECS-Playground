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
    }
    public View cubeView;

    public int spawnCount;

    public void OnAwake(ref SystemContext context)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var ent = Ent.New();
            var pos = UnityEngine.Random.insideUnitSphere * 25;
            var prefab = ViewsRegistry.GetEntityViewByPrefabId(cubeView.viewSource.prefabId);
            var go = Object.Instantiate(prefab, pos, Quaternion.identity);
            GOTransformSystem.Register(ent, go.transform);
            ent.Set(new Cube { speed = UnityEngine.Random.value * 5 });
            //ent.GetAspect<TransformAspect>().localPosition = ;
        }
    }

    public void OnUpdate(ref SystemContext context)
    {
        var query = API.Query(context).With<Cube>().WithAspect<TransformAspect>();

        foreach (var ent in query)
        {
            ref var cube = ref ent.Get<Cube>();
            var trs = ent.GetAspect<TransformAspect>();

            trs.localPosition += new float3(0, 0, cube.speed * context.deltaTime);
        }
    }
}
