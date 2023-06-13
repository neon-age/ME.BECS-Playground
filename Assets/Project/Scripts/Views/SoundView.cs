using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.Views;
using UnityEngine;

public class SoundView : EntityView
{
    public AudioSource source;
    public AudioClip[] clips;
    public Vector2 pitch = new Vector2(1, 1);

    protected override void OnEnableFromPool(in Ent ent)
    {
        var newClip = clips[Random.Range(0, clips.Length - 1)];
        source.clip = newClip;
        source.pitch = Random.Range(pitch.x, pitch.y);
        source.Play();

        ent.Get<LifetimeData>().value = newClip.length;
    }
}
