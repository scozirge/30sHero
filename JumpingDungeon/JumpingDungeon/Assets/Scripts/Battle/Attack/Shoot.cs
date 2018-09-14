using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Attack
{
    [Tooltip("飛射子彈物件")]
    [SerializeField]
    ShootAmmo AttackPrefab;


    protected override void SpawnAttackPrefab()
    {
        if (Target == null && Patetern == ShootPatetern.TowardTarget)
            return;
        if (AttackOnce && AttackTimes > 0)
            return;
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        ammo.transform.SetParent(AmmoParent);
        ammo.transform.position = transform.position;
        ammo.Init(AmmoData);
    }
}
