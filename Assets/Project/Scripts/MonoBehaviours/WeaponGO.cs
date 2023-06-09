using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public class WeaponGO : MonoBehaviour, IBeginTickInit
{
    public Weapon.Data weaponData;
    public GOEntity firePoint;
    public ParticlesEmitterGO muzzleFlashParticle;

    public void OnBeginTickInit(IBeginTickInit.Context ctx)
    {
        weaponData.firePoint = firePoint.ent;
        ctx.ent.Set(weaponData);
        ctx.ent.Get<Weapon.Data>().muzzleFlashParticle = muzzleFlashParticle;
        ctx.ent.Get<Weapon.State>();
    }

    void Start()
    {
        this.RegisterInitOnBeginTick();
    }
}
