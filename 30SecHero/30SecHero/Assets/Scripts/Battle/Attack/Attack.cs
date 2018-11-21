using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Role))]
public class Attack : Skill
{
    [Tooltip("是否只會攻擊一次")]
    [SerializeField]
    protected bool AttackOnce;
    [Tooltip("每次攻擊的間隔時間")]
    [SerializeField]
    protected float Interval;
    [Tooltip("每次攻擊發射的子彈數")]
    [SerializeField]
    protected int AmmoNum;
    [Tooltip("若子彈數不只一發，每發間隔的時間")]
    [SerializeField]
    protected float AmmoInterval;
    [Tooltip("起始發射角度")]
    [SerializeField]
    protected float StartAngle;
    [Tooltip("若子彈數不只一發，每發間隔的角度")]
    [SerializeField]
    protected float AngleInterval;
    [Tooltip("射擊模式")]
    [SerializeField]
    protected ShootPatetern Patetern;
    [Tooltip("子彈是否跟隨腳色本身跑")]
    [SerializeField]
    protected bool SpawnedInSelf;

    protected float Timer;
    protected float AmmoIntervalTimer;
    protected bool IsAttacking;
    protected int CurSpawnAmmoNum;
    bool IsPreAttack = false;
    protected Vector3 AttackDir = Vector3.zero;
    //protected float AmmoRotation;
    static protected float PreAttackTime = 1;
    float CurInterval;


    public override void LaunchAIAttack()
    {
        base.LaunchAIAttack();
        IsAttacking = true;
    }
    protected override void Awake()
    {
        base.Awake();
        InRange = false;

        if (gameObject.tag == Force.Enemy.ToString())
        {
            GameObject go = GameObject.FindGameObjectWithTag(Force.Player.ToString());
            if (go != null)
                Target = go.GetComponent<PlayerRole>();
        }
        CurInterval = Interval;
        Timer = Interval;
        if (SpawnedInSelf)
            AmmoParent = transform;
    }
    protected override void Update()
    {
        base.Update();
        AttackExecuteFunc();
    }
    public override void PlayerInitSkill()
    {
        Interval *= GameSettingData.SkillAmmoInterval;//玩家子彈發射間隔縮小
        base.PlayerInitSkill();
    }

    public override void SpawnAttackPrefab()
    {
        base.SpawnAttackPrefab();
        //Set AmmoData
        float origAngle;
        int reverse = 1;
        switch (Patetern)
        {
            case ShootPatetern.Default:
                float angle = (StartAngle + CurSpawnAmmoNum * AngleInterval) * Mathf.Deg2Rad;
                AttackDir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0).normalized;
                //AmmoRotation = (Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval)* Mathf.Deg2Rad;
                break;
            case ShootPatetern.LeftRight:
                if (Myself.DirectX == Direction.Right)
                {
                    reverse = 1;
                    AttackDir = Vector2.right;
                }
                else
                {
                    reverse = -1;
                    AttackDir = Vector2.left;
                }
                origAngle = ((Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval) * reverse) * Mathf.Deg2Rad;
                AttackDir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
                //AmmoRotation = (Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval * reverse) * Mathf.Deg2Rad;
                break;
            case ShootPatetern.TopDown:
                if (Myself.DirectY == Direction.Top)
                {
                    reverse = 1;
                    AttackDir = Vector2.up;
                }
                else
                {
                    reverse = -1;
                    AttackDir = Vector2.down;
                }
                origAngle = ((Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval) * reverse) * Mathf.Deg2Rad;
                AttackDir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
                //AmmoRotation = (Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval * reverse) * Mathf.Deg2Rad;
                break;
            case ShootPatetern.TowardTarget:
                AttackDir = (Target.transform.position - Myself.transform.position);
                origAngle = ((Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval)) * Mathf.Deg2Rad;
                AttackDir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
                //AmmoRotation = (Mathf.Atan2(AttackDir.y, AttackDir.x) * Mathf.Rad2Deg) + (StartAngle + CurSpawnAmmoNum * AngleInterval) * Mathf.Deg2Rad;
                break;
            case ShootPatetern.FaceDirection:
                if (Myself.RoleTrans.localScale.x > 0)
                    AttackDir = Vector2.right;
                else
                    AttackDir = Vector2.left;
                break;
        }

        if (gameObject.tag == Force.Player.ToString())
            AmmoData.Add("TargetRoleTag", Force.Enemy);
        else
        {
            AmmoData.Add("TargetRoleTag", Force.Player);
        }
        AmmoData.Add("Direction", AttackDir);
        AmmoData.Add("Target", Target);
        //AmmoData.Add("AmmoRotation", AmmoRotation);
        CurSpawnAmmoNum++;
        if (CurSpawnAmmoNum >= AmmoNum)
        {
            IsAttacking = false;
            CurSpawnAmmoNum = 0;
            AttackTimes++;
        }
    }
    bool InRange;
    protected override void TimerFunc()
    {
        if (BehaviorSkill)
            return;
        if (!CanAttack)
            return;
        if (!Target)
            return;
        if (Vector3.Distance(Target.transform.position, transform.position) <= DetecteRadius)
            InRange = true;
        if (!InRange)
            return;
        if (AttackOnce && AttackTimes > 0)
            return;
        base.TimerFunc();
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (!Myself.IsPreAttack)
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
            InRange = false;
            Timer = CurInterval;
        }
    }
    public override void Freeze(bool _freeze)
    {
        base.Freeze(_freeze);
        if (_freeze)
        {
            CurInterval = Interval * (1 + GameSettingData.FreezeMove);
            Timer *= (1 + GameSettingData.FreezeMove);
        }
        else
        {
            CurInterval = Interval;
            Timer *= 1 / (1 + GameSettingData.FreezeMove);
        }
        if (Timer > CurInterval)
            Timer = CurInterval;
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
            Myself.EndPreAttack();
        }
    }

}
