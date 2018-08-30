using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Attack
{
    [SerializeField]
    ShootPatetern Patetern;
    [SerializeField]
    Ammo AttackPrefab;

    protected override void SpawnAttackPrefab()
    {
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        ShootAmmo ea = ammoGO.GetComponent<ShootAmmo>();
        MyAmmos.Add(ea);
        ea.transform.SetParent(PrefabParent);
        ea.transform.position = transform.position;
        //Set AmmoData
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("Damage", Myself.Damage);
        data.Add("Target", Target);
        Vector3 dir;
        if (Patetern == ShootPatetern.TowardTarget)
        {
            dir = (Target.transform.position - Myself.transform.position).normalized;
        }
        else
        {
            float angle = StartAngle + CurSpawnAmmoNum * AngleInterval * Mathf.Deg2Rad;
            dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
        }
        data.Add("Direction", dir);
        ea.Init(data);
        CurSpawnAmmoNum++;
        if (CurSpawnAmmoNum >= AmmoNum)
        {
            IsAttacking = false;
            CurSpawnAmmoNum = 0;
        }
    }


}
