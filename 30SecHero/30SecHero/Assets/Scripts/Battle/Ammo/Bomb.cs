﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Ammo
{
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Launch();
    }
    protected override void TriggerTarget(Role _role,Collider2D _col)
    {
        if (_role.BuffersExist(RoleBuffer.Untouch))
            return;
        if (!TriggerOnRushRole && _role.OnRush)
            return;
        base.TriggerTarget(_role,_col);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        int damage = Value;
        _role.BeAttack(AttackerRoleTag,ref damage, force);
    }
}
