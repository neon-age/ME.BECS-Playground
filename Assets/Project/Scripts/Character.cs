using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using UnityEngine;

public struct CharacterState : IComponent
{
    [NonSerialized]
    public Ent weapon;
}
public struct CharacterData : IComponent, IConfigComponent
{
    public float moveSpeed;
}

public class Character : MonoBehaviour
{
    public EntityConfig config;
    public GOEntity weapon;

    void Start()
    {
        var ent = this.GetEntity();

        ent.Set(new CharacterState 
        {
            weapon = weapon.ent
        });
        config.Apply(ent);
    }
}
