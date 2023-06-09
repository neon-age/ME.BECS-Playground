using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

[DefaultExecutionOrder(-9997)]
public class GOEntityConfig : MonoBehaviour, IBeginTickInit
{
    public EntityConfig config;

    public void OnBeginTickInit(IBeginTickInit.Context ctx)
    {
        config.Apply(ctx.ent);
    }

    void Awake()
    {
        if (!config)
            return;
        var ent = this.GetEntity();
        this.RegisterInitOnBeginTick(ent);
    }
}
