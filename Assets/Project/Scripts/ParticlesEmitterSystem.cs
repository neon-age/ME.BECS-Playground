using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using UnityEngine;

public struct Particles
{
    public struct Shared : IComponentShared
    {
        public ObjectReference<ParticleSystem> particle;
    }
    public struct State : IComponent
    {
        public bool initialized;
    }
}

public class ParticlesEmitterSystem : IUpdate
{
    //public static Dictionary<int, ParticleSystem> ParticleSystemsLookup;

    public static Ent Emit(ParticlesEmitterGO emitter)
    {
        var ent = Ent.New();
        ent.GetShared<Particles.Shared>().particle = emitter.particle.Value;
        ent.Get<Particles.State>();
        return ent;
    }

    public void OnUpdate(ref SystemContext ctx)
    {
        var query = API.Query(ctx).With<Particles.State>();

        foreach (var ent in query)
        {
            var shared = ent.ReadShared<Particles.Shared>();
            var particle = shared.particle.Value;

            ref var state = ref ent.Get<Particles.State>();

            if (!state.initialized)
            {
                var trs = ent.GetAspect<TransformAspect>();

                var pos = trs.readLocalPosition;
                var rot = trs.readLocalRotation;
                var rotEuler = ((Quaternion)rot).eulerAngles;
                
                state.initialized = true;
                particle.Emit(new ParticleSystem.EmitParams { position = pos, rotation3D = rotEuler }, 1);
            }
        }
    }
}
