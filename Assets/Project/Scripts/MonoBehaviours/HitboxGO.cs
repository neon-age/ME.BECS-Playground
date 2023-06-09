using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using UnityEngine;

public struct HitboxLinkData : IComponent
{
    public Ent linkedEntity;
}

public class HitboxGO : MonoBehaviour
{
    public GOEntity target;

    void Start()
    {
        var ent = this.GetEntity();
        ent.Get<HitboxLinkData>().linkedEntity = target ? target.ent : ent;
    }
}
