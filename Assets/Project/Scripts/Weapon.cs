using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public class Weapon : MonoBehaviour, IBeginTickInit
{
    public WeaponData weaponData;
    public GOEntity firePoint;

    public void OnBeginTickInit(Ent ent, object userData)
    {
        weaponData.firePoint = firePoint.ent;
        ent.Set(weaponData);
        ref var state = ref ent.Get<WeaponState>();
    }

    void Start()
    {
        this.RegisterInitOnBeginTick();
    }
}
