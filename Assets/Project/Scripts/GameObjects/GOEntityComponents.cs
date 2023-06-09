using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;


// TODO: Can't use until we have drawer for ComponentsStorage
[DefaultExecutionOrder(-9998)]
[AddComponentMenu("")]
class GOEntityComponents : MonoBehaviour, IBeginTickInit
{
    public ComponentsStorage<IComponent> components;

    public void OnBeginTickInit(IBeginTickInit.Context ctx)
    {
        components.Apply(ctx.ent);
    }

    void Start()
    {
        this.RegisterInitOnBeginTick();
    }
}
