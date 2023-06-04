using System;
using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class GOEntity : MonoBehaviour
{
    public Ent ent;
    [NonSerialized] public Transform trs;
    
    void Awake()
    {
        trs = transform;
        ent = this.GetEntity();
    }
}
