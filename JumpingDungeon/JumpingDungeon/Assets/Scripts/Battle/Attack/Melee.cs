using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attack
{
    [SerializeField]
    ParticleSystem[] PrepareParticle;
    [SerializeField]
    MeleeAmmo AttackPrefab;
    [SerializeField]
    protected float AttackRadius;

    protected override void SpawnAttackPrefab()
    {
        if (Target == null && Patetern == ShootPatetern.TowardTarget)
            return;
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        ammo.transform.SetParent(AmmoParent);
        ammo.transform.position = transform.position + AttackRadius * AttackDir;
        ammo.Init(AmmoData);
    }
}
