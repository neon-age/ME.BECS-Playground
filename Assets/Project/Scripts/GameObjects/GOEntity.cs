using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GOEntity : MonoBehaviour
{
    [HideInInspector] // UI Elements shit is catastrophically slow :( enable at your own risk
    public Ent ent;
    [HideInInspector]
    public Ent parent;

    public bool syncTransforms = true;

    [NonSerialized] public Transform trs;
    
    void Awake() // first phase, get entities
    {
        trs = transform;
        ent = this.GetEntity();

        var trsParent = trs.parent;
        if (trsParent)
        {
            parent = trsParent.GetEntity();
        }

        // TODO: Register transform doesn't work in Awake? No entities found by query
    }

    void Start() // second phase, register transforms
    {
        if (syncTransforms)
        {
            GOTransformSystem.Register(ent, trs);

            if (!parent.IsEmpty())
            {
                ent.SetParent(parent);
            }
        }
    }

    void OnDestroy()
    {
        GOEntityLookup.RemoveEntity(trs);
        if (!ent.IsAlive())
            return;
        if (syncTransforms)
            GOTransformSystem.Unregister(ent);
    }
}
