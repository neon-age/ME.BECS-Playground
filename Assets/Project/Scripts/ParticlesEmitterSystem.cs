using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using Unity.Mathematics;
using UnityEngine;

public struct Particles
{
    public struct State : IComponent
    {
        public Ent parent;
        public bool initialized;
    }
}

public struct ParticlesEmitterSystem : IUpdate
{
    class ParticleSystemBuffer
    {
        public ParticleSystem particleSystem;
        public ParticleSystem.Particle[] particles;
        public Ent[] entities;
        public int particlesCount;

        public void Add(Ent ent, float lifetime)
        {
            if (particles.Length >= particlesCount)
            {
                var newSize = particlesCount * 2 + 10;
                System.Array.Resize(ref particles, newSize);
                System.Array.Resize(ref entities, newSize);
            }
            particles[particlesCount] = new ParticleSystem.Particle
            {
                remainingLifetime = lifetime,
                startLifetime = lifetime,
            };
            entities[particlesCount] = ent;
            particlesCount++;
        }
        public void RemoveAtSwapBack(int i)
        {
            particlesCount--;
            particles[i] = particles[particlesCount];
            entities[i] = entities[particlesCount];
        }
    }
    static ParticleSystemBuffer[] ParticleSystems = new ParticleSystemBuffer[0];
    static Dictionary<ParticleSystem, int> ParticleSystemIds = new Dictionary<ParticleSystem, int>();
    static int ParticleSystemsCount;

    public static Ent Emit(ParticleSystem particleSystem, float lifetime, float3 pos, quaternion rot, Ent parent = default)
    {
        if (!ParticleSystemIds.TryGetValue(particleSystem, out var particleSystemId))
            ParticleSystemIds.Add(particleSystem, particleSystemId = ParticleSystemsCount++);

        var ent = Ent.New();
        ent.Transform().LocalPositionRotation(pos, rot);
        ent.Get<Particles.State>().parent = parent;
        ent.Get<LifetimeData>().value = lifetime;

        if (particleSystemId >= ParticleSystems.Length)
            System.Array.Resize(ref ParticleSystems, particleSystemId + 1);

        ref var buffer = ref ParticleSystems[particleSystemId];
        if (buffer == null)
        {
            buffer = new ParticleSystemBuffer() 
            { 
                particleSystem = particleSystem,
                particles = new ParticleSystem.Particle[0],
                entities = new Ent[0],
            };
        }
        buffer.Add(ent, lifetime);
        return ent;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void OnLoad()
    {
        ParticleSystemsCount = 0;
        ParticleSystems = new ParticleSystemBuffer[0];
        ParticleSystemIds.Clear();
    }

    public void OnUpdate(ref SystemContext ctx)
    {
        for (int p = 0; p < ParticleSystemsCount; p++)
        {
            var buffer = ParticleSystems[p];
            var particleSystem = buffer.particleSystem;

            for (int e = 0; e < buffer.particlesCount; e++)
            {
                var ent = buffer.entities[e];
                if (!ent.IsAlive())
                {
                    buffer.RemoveAtSwapBack(e);
                    continue;
                }
                ref var particle = ref buffer.particles[e];
                var state = ent.Read<Particles.State>();
                var lifetime = ent.Read<LifetimeData>();

                particle.remainingLifetime = lifetime.value;

                var hasParent = !state.parent.IsEmpty() && state.parent.IsAlive();

                //if (!state.initialized || hasParent)
                {
                    var trs = (hasParent ? state.parent : ent).Transform();

                    trs.Position(out var pos).Rotation(out var rot);

                    // This won't work in case where you'd need "burst" particles to follow the emitter, 
                    // since sub-emitters don't support following position (as would be in local space), only velocity can be inherited :(
                    particle.position = pos;

                    // https://forum.unity.com/threads/sub-emitter-shape-rotation-inherit-from-parent-particle-rotation.623671/
                    // Sub emitter shape inherits its orientation based on the velocity of the parent particle, not it’s rotation.
                    particle.velocity = math.mul(rot, new float3(0, 0, 0.01f));
                }

                if (!state.initialized)
                {
                    particleSystem.TriggerSubEmitter(0, ref particle);

                    state.initialized = true;
                    ent.Set(state);
                }
            }

            buffer.particleSystem.SetParticles(buffer.particles, buffer.particlesCount, 0);
        }
    }
}
