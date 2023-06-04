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

    [NonSerialized] public Transform trs;
    
    void Awake() // first phase
    {
        trs = transform;
        ent = this.GetEntity();

        var trsParent = trs.parent;
        if (trsParent)
        {
            parent = trsParent.GetEntity();
        }
    }

    void Start() // second phase
    {
        if (!parent.IsEmpty())
        {
            var trsAspect = ent.GetAspect<TransformAspect>();

            trsAspect.localPosition = transform.localPosition;
            trsAspect.localRotation = transform.localRotation;
            trsAspect.localScale = transform.localScale;

            ent.SetParent(parent);
        }
    }

    void OnDestroy()
    {
        GOEntityLookup.RemoveEntity(trs);
    }
}
