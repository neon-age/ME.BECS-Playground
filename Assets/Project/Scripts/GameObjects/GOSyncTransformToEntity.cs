using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class GOSyncTransformToEntity : MonoBehaviour
{
    public GOEntity parent;
    public GOTransform.SyncGO sync;

    void Start()
    {
        var ent = this.GetEntity();

        GOTransformSystem.Register(ent, transform);
        ent.Get<GOTransform>().syncGO = sync;

        if (parent)
        {
            ent.Get<ParentComponent>().value = parent.ent;
        }
    }
}
