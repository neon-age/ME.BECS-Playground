using System.Collections;
using System.Collections.Generic;
using ME.BECS;
using ME.BECS.TransformAspect;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    public GOEntity firePoint;

    void Start()
    {
        var ent = this.GetEntity();

        weaponData.firePoint = firePoint.ent;

        ent.Set(weaponData);
        ent.Get<WeaponState>();
    }
}
