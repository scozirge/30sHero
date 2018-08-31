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
        switch (_col.gameObject.tag)
        {
            case "Player":
                PlayerRole pr = _col.GetComponent<PlayerRole>();
                Vector2 force = (pr.transform.position - transform.position).normalized * KnockIntensity;
                Dictionary<RoleBuffer, BufferData> condition = new Dictionary<RoleBuffer, BufferData>();
                condition.Add(RoleBuffer.Stun, new BufferData(StunIntensity, 0));
                pr.BeAttack(Damage, force, condition);
                ReadyToDamage = false;
                if(!ContinuousDamage)
                    EndDamage = true;
                break;
            default:
                break;
        }
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
