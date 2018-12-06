using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Attack
{
    [Tooltip("飛射子彈物件")]
    [SerializeField]
    ShootAmmo AttackPrefab;
    [Tooltip("是否會被牆阻擋")]
    [SerializeField]
    protected bool TriggerWall = true;


    public override void SpawnAttackPrefab()
    {
        if (Target == null && Patetern == ShootPatetern.TowardTarget)
            return;
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        ammo.transform.SetParent(AmmoParent);
        ammo.transform.position = transform.position;
        if (IsPlayerGetSkill)
            ammo.IsPlayerGetSkill = true;
        AmmoData.Add("TriggerWall", TriggerWall);
        ammo.Init(AmmoData);
    }
}
