using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Views;
using UnityEngine;

public class SoundView : EntityView
{
    public AudioSource source;
    public AudioClip[] clips;

    protected override void OnEnableFromPool(in Ent ent)
    {
        var newClip = clips[Random.Range(0, clips.Length - 1)];
        source.clip = newClip;
        source.Play();

        ent.Get<LifetimeData>().value = newClip.length;
    }
}
