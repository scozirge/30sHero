using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Role))]
public class Attack : MonoBehaviour
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

    protected Role Myself;
    protected Role Target;
    protected float Timer;
    protected float AmmoIntervalTimer;
    protected bool IsAttacking;
    protected int CurSpawnAmmoNum;
    protected Transform AmmoParent;
    protected Dictionary<string, object> AmmoData = new Dictionary<string, object>();

    protected virtual void Awake()
    {
        AmmoParent = GameObject.FindGameObjectWithTag("AmmoParent").transform;
        if (gameObject.tag == Force.Player.ToString())
        {
            Myself = GetComponent<PlayerRole>();
        }
        else if (gameObject.tag == Force.Enemy.ToString())
        {
            Myself = GetComponent<EnemyRole>();
            Target = GameObject.FindGameObjectWithTag(Force.Player.ToString()).GetComponent<PlayerRole>();
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
        if (gameObject.tag == Force.Player.ToString())
            Target = GameobjectFinder.FindClosestGameobjectWithTag(gameObject, Force.Enemy.ToString()).GetComponent<EnemyRole>();
    }
    protected virtual void SpawnAttackPrefab()
    {
        AmmoData.Clear();
        AmmoData.Add("AttackerForce", Myself.MyForce);
    }
    protected virtual void TimerFunc()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
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

}
