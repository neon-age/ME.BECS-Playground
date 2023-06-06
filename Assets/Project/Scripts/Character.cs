using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Addons;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using UnityEngine;

public struct CharacterState : IComponent
{
    //[NonSerialized]
    public Ent weapon;
}
public struct CharacterData : IComponent, IConfigComponent
{
    [NonSerialized]
    public ObjectReference<Rigidbody> body;

    public float moveSpeed;
    public float moveDirChangeVel;
    public float maxVelocity;
    public float lookSpring;
    public float lookDamper;
}

public class Character : MonoBehaviour, IBeginTickInit
{
    public GOEntity weapon;
    public Rigidbody body;

    public void OnBeginTickInit(Ent ent, object userData)
    {
        ent.Set(new CharacterState
        {
            weapon = weapon.ent
        });
        ent.Get<CharacterData>().body = body;
    }

    void Start()
    {
        this.RegisterInitOnBeginTick();
    }
}
