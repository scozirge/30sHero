using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Ammo
{
    [SerializeField]
    bool ContinuousDamage;
    [SerializeField]
    float DamageInterval;

    protected bool ReadyToDamage;
    protected float DamageIntervalTimer;
    protected bool EndDamage;

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
    protected override void TriggerTarget(Role _curTarget)
    {
        base.TriggerTarget(_curTarget);
        Vector2 force = (_curTarget.transform.position - transform.position).normalized * KnockIntensity;
        Dictionary<RoleBuffer, BufferData> condition = new Dictionary<RoleBuffer, BufferData>();
        condition.Add(RoleBuffer.Stun, new BufferData(StunIntensity, 0));
        _curTarget.BeAttack(Damage, force, condition);
        ReadyToDamage = false;
        if (!ContinuousDamage)
            EndDamage = true;
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
        if (EndDamage)
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
