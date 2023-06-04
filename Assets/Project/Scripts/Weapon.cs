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

        var firePointTrs = firePoint.ent.GetAspect<TransformAspect>();
        firePointTrs.localPosition = firePoint.trs.localPosition;
        firePointTrs.localRotation = firePoint.trs.localRotation;
        firePoint.ent.Get<ParentComponent>().value = ent;
    }
}
