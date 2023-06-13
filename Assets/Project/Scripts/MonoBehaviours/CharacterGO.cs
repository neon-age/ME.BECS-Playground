using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using Unity.Mathematics;
using UnityEngine;

public class CharacterGO : MonoBehaviour, IBeginTickInit
{
    public GOEntity weapon;
    public Rigidbody body;
    public View collisionSound;
    public View deathSound;
    public GlobalGOHandle collisionParticle;
    public GlobalGOHandle deathParticle;
    public float minCollisionSoundVelocity;

    ContactPoint collisionContact;
    int actualCollisionCount;
    int callbackCollisionCount;

    public void OnBeginTickInit(IBeginTickInit.Context ctx)
    {
        ctx.ent.Set(new Character.State
        {
            deathParticle = deathParticle,
            deathSound = deathSound,
            weapon = weapon.ent,
            body = body,
        });
        ctx.ent.Get<Character.Inputs>();
    }

    public void OnCollisionEnterTick(IBeginTickInit.Context ctx)
    {
        if (collisionContact.impulse.magnitude < minCollisionSoundVelocity)
            return;
        var soundEnt = Ent.New();

        var point = collisionContact.point;
        soundEnt.Transform().LocalPosition(point);
        soundEnt.InstantiateView(collisionSound);

        var particle = collisionParticle.GetInstance().GetComponent<ParticleSystem>();

        ParticlesEmitterSystem.Emit(particle, 1, point, Quaternion.identity);
    }

    void Start()
    {
        this.RegisterInitOnBeginTick();
    }

    void OnCollisionEnter(Collision collision)
    {
        actualCollisionCount++;
        collisionContact = collision.GetContact(0);
        this.RegisterActionOnBeginTick(OnCollisionEnterTick);
    }
}
