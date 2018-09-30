using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Ammo
{
    [Tooltip("傷害間隔，只有子彈類型是穿透(代表擊中敵方後不會消失)才需要設定")]
    [SerializeField]
    float DamageInterval;


    protected bool ReadyToDamage;
    protected float DamageIntervalTimer;

    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        ReadyToDamage = true;
        Launch();
        DamageIntervalTimer = DamageInterval;
    }
    protected override void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToDamage)
            return;
        base.OnTriggerStay2D(_col);
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (!ReadyToDamage)
            return;
        base.OnTriggerEnter2D(_col);
    }
    protected override void TriggerTarget(Role _role)
    {
        base.TriggerTarget(_role);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        _role.BeAttack(Value, force);
        TriggerHitCondition(_role);
        ReadyToDamage = false;
        if (AmmoType != ShootAmmoType.Permanent)
            IsCausedDamage = true;
    }
    protected override void Update()
    {
        base.Update();
        DamageINtervalTimerFunc();
    }
    public override void Launch()
    {
        base.Launch();
    }
    protected void DamageINtervalTimerFunc()
    {
        if (ReadyToDamage)
            return;
        if (IsCausedDamage)
            return;
        if (DamageIntervalTimer > 0)
        {
            DamageIntervalTimer -= Time.deltaTime;
        }
        else
        {
            DamageIntervalTimer = DamageInterval;
            ReadyToDamage = true;
        }
    }
}
