using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Attack
{
    [SerializeField]
    ShootPatetern Patetern;
    [SerializeField]
    ShootAmmo AttackPrefab;

    protected override void SpawnAttackPrefab()
    {
        if (Target == null && Patetern == ShootPatetern.TowardTarget)
            return;
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        ammo.transform.SetParent(AmmoParent);
        ammo.transform.position = transform.position;
        //Set AmmoData
        AmmoData.Add("Damage", Myself.Damage);
        AmmoData.Add("Target", Target);
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
