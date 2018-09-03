using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attack
{
    [SerializeField]
    ShootPatetern Patetern;
    [SerializeField]
    ParticleSystem[] PrepareParticle;
    [SerializeField]
    MeleeAmmo AttackPrefab;
    [SerializeField]
    protected float AttackRadius;

    protected override void SpawnAttackPrefab()
    {
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        ammo.transform.SetParent(AmmoParent);
        //Set AmmoData
        AmmoData.Add("Damage", Myself.Damage);
        Vector3 dir;
        if (Patetern == ShootPatetern.TowardTarget)
        {
            dir = (Target.transform.position - Myself.transform.position);
            float origAngle = ((Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval)) * Mathf.Deg2Rad;
            dir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
        }
        else
        {
            float angle = (StartAngle + CurSpawnAmmoNum * AngleInterval) * Mathf.Deg2Rad;
            dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
        }
        ammo.transform.position = transform.position + AttackRadius * dir;
        AmmoData.Add("Direction", dir);
        ammo.Init(AmmoData);
        CurSpawnAmmoNum++;
        if (CurSpawnAmmoNum >= AmmoNum)
        {
            IsAttacking = false;
            CurSpawnAmmoNum = 0;
        }
    }
}
