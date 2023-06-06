using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

[DefaultExecutionOrder(-9999)]
public class GOEntityComponents : MonoBehaviour, IBeginTickInit
{
    public ComponentsStorage<IComponent> components;

    public void OnBeginTickInit(Ent ent, object userData)
    {
        components.Apply(ent);
    }

    void Start()
    {
        this.RegisterInitOnBeginTick();
    }
}
