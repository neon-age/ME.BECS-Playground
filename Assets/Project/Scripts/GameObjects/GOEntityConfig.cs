using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

[DefaultExecutionOrder(-9999)]
public class GOEntityConfig : MonoBehaviour
{
    public EntityConfig config;

    void Awake()
    {
        if (!config)
            return;
        var ent = this.GetEntity();
        config.Apply(ent);
    }

}
