using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Role))]
public class Attack : Skill
{

    [SerializeField]
    protected float Interval;
    [SerializeField]
    protected int AmmoNum;
    [SerializeField]
    protected float AmmoInterval;
    [SerializeField]
    protected float StartAngle;
    [SerializeField]
    protected float AngleInterval;
    [SerializeField]
    protected ShootPatetern Patetern;


    protected float Timer;
    protected float AmmoIntervalTimer;
    protected bool IsAttacking;
    protected int CurSpawnAmmoNum;
    protected Role Target;
    protected Vector3 AttackDir = Vector3.zero;
    static protected float PreAttackTime = 1;

    protected override void Awake()
    {
        base.Awake();
        if (Myself.MyForce == Force.Enemy)
        {
            GameObject go = GameObject.FindGameObjectWithTag(Force.Player.ToString());
            if (go != null)
                Target = go.GetComponent<PlayerRole>();
        }
        Timer = Interval;
    }
    protected virtual void Update()
    {
        AutoDetectTarge();
        TimerFunc();
        AttackExecuteFunc();
    }
    protected virtual void AutoDetectTarge()
    {
        if (Myself.MyForce == Force.Player)
        {
            GameObject go = GameobjectFinder.FindClosestGameobjectWithTag(gameObject, Force.Enemy.ToString());
            if (go != null)
                Target = go.GetComponent<EnemyRole>();
        }
    }
    protected override void SpawnAttackPrefab()
    {
        base.SpawnAttackPrefab();
        AmmoData.Add("Target", Target);
        //Set AmmoData
        if (Patetern == ShootPatetern.TowardTarget)
        {
            AttackDir = (Target.transform.position - Myself.transform.position);
            float origAngle = ((Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval)) * Mathf.Deg2Rad;
            AttackDir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
        }
        else
        {
            float angle = (StartAngle + CurSpawnAmmoNum * AngleInterval) * Mathf.Deg2Rad;
            AttackDir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
        }
        AmmoData.Add("Direction", AttackDir);

        CurSpawnAmmoNum++;
        if (CurSpawnAmmoNum >= AmmoNum)
        {
            IsAttacking = false;
            CurSpawnAmmoNum = 0;
        }
    }
    protected virtual void TimerFunc()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= PreAttackTime)
                Myself.PreAttack();
        }
        else
        {
            IsAttacking = true;
            Timer = Interval;
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
    public override void PlayerGetSkill()
    {
        base.PlayerGetSkill();
        Awake();
    }

}
