using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public interface IBeginTickInit 
{
    void OnBeginTickInit(Ent ent, object userData);
}

[DefaultExecutionOrder(-10000)]
public class GOEntity : MonoBehaviour, IBeginTickInit
{
    [HideInInspector] // UI Elements shit is catastrophically slow :( enable at your own risk
    public Ent ent;
    [HideInInspector]
    public Ent parent;

    public Config config;

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
    }

    public void OnBeginTickInit(Ent ent, object userData)
    {
        GOTransformSystem.Register(ent, parent, trs, config);

        ref var trsCache = ref ent.Get<GOTransformSystem.TransformCache>();
        trsCache.localPos = trs.localPosition;
        trsCache.localRot = trs.localRotation;
        trsCache.localScale = trs.localScale;

        var trsAspect = ent.GetAspect<TransformAspect>();
        trsAspect.localPosition = trsCache.localPos;
        trsAspect.localRotation = trsCache.localRot;
        trsAspect.localScale = trsCache.localScale;
        
        if (!parent.IsEmpty())
            ent.SetParent(parent);

        if (config.sourceId != 0)
            config.Apply(ent);
    }

    void Start() // second phase, register transforms
    {
        this.RegisterInitOnBeginTick(ent);

        //if (!parent.IsEmpty())
        //{
        //    ent.SetParent(parent);
        //}
    }

    void OnDestroy()
    {
        GOEntityLookup.RemoveEntity(trs);
        if (ent.IsAlive())
        {
            GOTransformSystem.Unregister(ent);
            ent.Destroy();
            //ent.RegisterAction(() =>
            //{
            //    ent.Destroy();
            //});
        }
    }
}
