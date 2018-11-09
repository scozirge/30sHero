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
    [Tooltip("是否可以對自己施放(對自己施放不算在目標數內)")]
    [SerializeField]
    bool SelfTarget;
    [Tooltip("是否會對擁有support這個技能的目標施放")]
    [SerializeField]
    bool SpellToSupportTarget;

    protected float Timer;
    protected float AmmoIntervalTimer;
    protected bool IsAttacking;
    protected int CurSpawnAmmoNum;
    protected List<Role> SupportTargets;
    bool IsPreAttack = false;
    protected Vector3 AttackDir = Vector3.zero;
    static protected float PreAttackTime = 1;
    float CurInterval;

    protected override void Awake()
    {
        base.Awake();
        SupportTargets = new List<Role>();
        CurInterval = Interval;
        Timer = Interval;
    }

    protected override void Update()
    {
        TimerFunc();
        AttackExecuteFunc();
    }
    protected override void AutoDetectTarge()
    {
        //base.AutoDetectTarge();
        if (gameObject.tag == Force.Enemy.ToString())
        {
            SupportTargets = new List<Role>();
            List<GameObject> gos;
            if (SpellToSupportTarget)
                gos = GameobjectFinder.FindInRangeClosestGameobjectsWithTag(gameObject, Force.Enemy.ToString(), TargetCount, DetecteRadius);
            else
                gos = GameobjectFinder.FindInRangeClosestNonSupporterWithTag(gameObject, Force.Enemy.ToString(), TargetCount, DetecteRadius);
            if (gos != null)
            {
                for (int i = 0; i < gos.Count; i++)
                {
                    SupportTargets.Add(gos[i].GetComponent<EnemyRole>());
                }
            }
        }
    }
    public override void SpawnAttackPrefab()
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
        if (BehaviorSkill)
            return;
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
            Timer = CurInterval;
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
            AutoDetectTarge();
            Myself.EndPreAttack();
            SepllToMyself();
            SpawnAttackPrefab();
        }
    }
    void SepllToMyself()
    {
        //對自己施放
        if (SelfTarget)
        {
            //Set AmmoData
            base.SpawnAttackPrefab();
            AmmoData.Add("Direction", Vector3.zero);
            AmmoData.Add("Attacker", Myself);
            AmmoData.Add("TargetRoleTag", Myself.MyForce);
            AmmoData.Add("Target", Myself);
            GameObject ammoGO = Instantiate(SupplyPrefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
            Ammo ammo = ammoGO.GetComponent<Ammo>();
            ammo.transform.SetParent(AmmoParent);
            ammo.transform.position = transform.position;
            ammo.Init(AmmoData);
        }
    }
    public override void PlayerInitSkill()
    {
        base.PlayerInitSkill();
        SelfTarget = true;
    }
}
