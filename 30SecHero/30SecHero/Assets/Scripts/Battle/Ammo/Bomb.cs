using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Ammo
{
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Launch();
    }
    protected override void TriggerTarget(Role _role)
    {
        base.TriggerTarget(_role);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        _role.BeAttack(AttackerRoleTag, Value, force);
    }
}
