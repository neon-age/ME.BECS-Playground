using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS.Addons;
using UnityEngine;

public class ParticlesEmitterGO : MonoBehaviour
{
    public ParticlesEmitterGO prefab;
    [NonSerialized]
    public ObjectReference<ParticleSystem> particle;

    public void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        prefab.particle = particle;
    }
}
