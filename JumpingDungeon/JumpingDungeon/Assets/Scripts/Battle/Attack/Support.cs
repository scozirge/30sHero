using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Role))]
public class Support : Skill
{
    [Tooltip("是否只會施放一次")]
    [SerializeField]
    protected bool SupportOnce;
    [Tooltip("每次施放的間隔時間")]
    [SerializeField]
    protected float Interval;
    [Tooltip("目標數")]
    [SerializeField]
    protected int TargetCount;
    [Tooltip("若目標數不只一位，每位間隔的時間")]
    [SerializeField]
    protected float AmmoInterval;
    [Tooltip("Supply子彈物件")]
    [SerializeField]
    Supply SupplyPrefab;

    protected float Timer;
    protected float AmmoIntervalTimer;
    protected bool IsAttacking;
    protected int CurSpawnAmmoNum;
    protected List<Role> SupportTargets;
    bool IsPreAttack = false;
    protected Vector3 AttackDir = Vector3.zero;
    static protected float PreAttackTime = 1;

    protected override void Awake()
    {
        base.Awake();
        SupportTargets = new List<Role>();
        Timer = Interval;
    }

    protected override void Update()
    {
        base.Update();
        AttackExecuteFunc();
    }
    protected override void AutoDetectTarge()
    {
        //base.AutoDetectTarge();
        if (gameObject.tag == Force.Enemy.ToString())
        {
            SupportTargets = new List<Role>();
            List<GameObject> gos = GameobjectFinder.FindInRangeClosestGameobjectsWithTag(gameObject, Force.Enemy.ToString(), TargetCount, DetecteRadius);
            for (int i = 0; i < gos.Count; i++)
            {
                SupportTargets.Add(gos[i].GetComponent<EnemyRole>());
            }
        }
    }
    protected override void SpawnAttackPrefab()
    {
        if (CurSpawnAmmoNum >= SupportTargets.Count)
        {
            IsAttacking = false;
            CurSpawnAmmoNum = 0;
            return;
        }
        base.SpawnAttackPrefab();
        //Set AmmoData
        AttackDir = (SupportTargets[CurSpawnAmmoNum].transform.position - Myself.transform.position);
        float origAngle = (Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) * Mathf.Deg2Rad;
        AttackDir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
        AmmoData.Add("Direction", AttackDir);
        AmmoData.Add("Attacker", Myself);
        if (gameObject.tag == Force.Player.ToString())
            AmmoData.Add("TargetRoleTag", SupportTargets[CurSpawnAmmoNum].MyForce);
        else
            AmmoData.Add("TargetRoleTag", SupportTargets[CurSpawnAmmoNum].MyForce);
        AmmoData.Add("Target", SupportTargets[CurSpawnAmmoNum]);
        GameObject ammoGO = Instantiate(SupplyPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
        Ammo ammo = ammoGO.GetComponent<Ammo>();
        ammo.transform.SetParent(AmmoParent);
        ammo.transform.position = transform.position;
        ammo.Init(AmmoData);

        CurSpawnAmmoNum++;
    }
    protected override void TimerFunc()
    {
        if (!CanAttack)
            return;
        if (SupportTargets.Count < 0)
            return;
        if (SupportOnce && AttackTimes > 0)
            return;
        base.TimerFunc();
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (!IsPreAttack)
                if (Timer <= PreAttackTime)
                    if (Myself.MyForce == Force.Enemy)
                    {
                        Myself.PreAttack();
                        IsPreAttack = true;
                    }
        }
        else
        {
            IsAttacking = true;
            IsPreAttack = false;
            Timer = Interval;
            AttackTimes++;
        }
    }
    protected virtual void AttackExecuteFunc()
    {
        if (!IsAttacking)
            return;
        if (AmmoIntervalTimer > 0)
        {
            AmmoIntervalTimer -= Time.deltaTime;
        }
        else
        {
            SpawnAttackPrefab();
            AmmoIntervalTimer = AmmoInterval;
        }
    }

}
