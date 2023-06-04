using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using ME.BECS.Views;
using UnityEngine;

public struct CharacterState : IComponent
{
    public Ent weapon;
}

public class Character : MonoBehaviour
{
    public struct Data : IComponent, IConfigComponent
    {
        public float moveSpeed;
    }
    public EntityConfig config;
    public GOEntity weapon;

    void Start()
    {
        var ent = this.GetEntity();

        ent.Set(new CharacterState 
        {
            weapon = weapon.ent
        });
        ent.GetAspect<TransformAspect>();
        config.Apply(ent);

        GOTransformSystem.Register(ent, transform);
    }
}
