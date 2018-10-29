using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Attack
{
    [Tooltip("肉搏子彈物件")]
    [SerializeField]
    MeleeAmmo AttackPrefab;
    [Tooltip("子彈產生半徑(距離攻擊者多少距離)")]
    [SerializeField]
    protected float AttackRadius;

    public override void SpawnAttackPrefab()
    {
        if (Target == null && Patetern == ShootPatetern.TowardTarget)
            return;
        if (AttackOnce && AttackTimes > 0)
            return;
        base.SpawnAttackPrefab();
        GameObject ammoGO = Instantiate(AttackPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        AmmoData.Add("Attacker", Myself);
        ammo.transform.SetParent(AmmoParent);
        ammo.transform.position = transform.position + AttackRadius * AttackDir;
        ammo.Init(AmmoData);
        SubordinateAmmos.Add(ammo);
    }
}
