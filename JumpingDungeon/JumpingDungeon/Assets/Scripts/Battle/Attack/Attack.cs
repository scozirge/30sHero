using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyRole))]
public class Attack : MonoBehaviour
{
    [SerializeField]
    protected Element Element;

    [SerializeField]
    protected Transform PrefabParent;
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

    protected EnemyRole Myself;
    protected PlayerRole Target;
    protected float Timer;
    protected float AmmoIntervalTimer;
    protected bool IsAttacking;
    protected int CurSpawnAmmoNum;
    protected List<ShootAmmo> MyAmmos;

    protected virtual void Awake()
    {
        Myself = GetComponent<EnemyRole>();
        Target = GameObject.FindGameObjectWithTag(Force.Player.ToString()).GetComponent<PlayerRole>();
        Timer = Interval;
    }
    protected virtual void Update()
    {
        TimerFunc();
        AttackExecuteFunc();
    }
    protected virtual void SpawnAttackPrefab()
    {
        //Spawn ammo
        MyAmmos = new List<ShootAmmo>();
    }
    protected virtual void TimerFunc()
    {
        if(Timer>0)
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
