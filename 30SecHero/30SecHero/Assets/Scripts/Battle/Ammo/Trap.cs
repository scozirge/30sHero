using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Ammo
{
    [SerializeField]
    float DamageHPRatio;
    void Start()
    {
        Dictionary<string, object> dataDic = new Dictionary<string, object>();
        dataDic.Add("AttackerForce", Force.Enemy);
        dataDic.Add("TargetRoleTag", Force.Player);
        dataDic.Add("Damage", 0);
        Init(dataDic);
        OutSideDestroy = false;
    }
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Launch();
    }
    protected override void LIfeTimerFunc()
    {
    }
    protected override void TriggerTarget(Role _role, Vector2 _pos)
    {
        if (_role.BuffersExist(RoleBuffer.Untouch))
            return;
        if (!TriggerOnRushRole && _role.OnRush)
            return;
        base.TriggerTarget(_role, _pos);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        Value = (int)(_role.MaxHealth * DamageHPRatio);
        int damage = Value;
        _role.BeAttack(AttackerRoleTag, ref damage, force);
    }
}
