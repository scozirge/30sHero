using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAmmo : Ammo
{
    [Tooltip("Melee是一般肉搏,Block是擋子彈,Reflect是檔子彈+也可以攻擊人,Mirror是擋子彈+攻擊人+反彈ShootAmmo類型的攻擊")]
    [SerializeField]
    MeleeType MyMeleeType;
    [Tooltip("格擋強度，1是全部格擋，0.1是只檔10%傷害")]
    [SerializeField]
    protected float BlockIntensity;

    Role Attacker;
    protected AmmoForce TargetAmmoForce;

    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        if (MyMeleeType == MeleeType.Block || MyMeleeType == MeleeType.Reflect)
        {
            if (AttackerRoleTag == Force.Player)
                TargetAmmoForce = AmmoForce.EnemyAmmo;
            else
                TargetAmmoForce = AmmoForce.PlayerAmmo;
        }
        Vector3 dir = (Vector3)(_dic["Direction"]);
        Attacker = (Role)(_dic["Attacker"]);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (BlockIntensity > 1)
            BlockIntensity = 1;
        else if (BlockIntensity < 0)
            BlockIntensity = 0;
        Launch();
    }
    protected override void OnTriggerStay2D(Collider2D _col)
    {
        if (IsCausedDamage && AmmoType != ShootAmmoType.Penetration)
            return;
        base.OnTriggerStay2D(_col);
        if (TargetAmmoForce.ToString() == _col.tag.ToString())
            TriggerAmmo(_col.GetComponent<Ammo>());
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (IsCausedDamage && AmmoType != ShootAmmoType.Penetration)
            return;
        base.OnTriggerEnter2D(_col);
        if (TargetAmmoForce.ToString() == _col.tag.ToString())
            TriggerAmmo(_col.GetComponent<Ammo>());
    }
    protected override void TriggerTarget(Role _role)
    {
        base.TriggerTarget(_role);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        Dictionary<RoleBuffer, BufferData> condition = new Dictionary<RoleBuffer, BufferData>();
        condition.Add(RoleBuffer.Stun, new BufferData(StunIntensity, 0));
        if (MyMeleeType == MeleeType.Melee || MyMeleeType == MeleeType.Reflect || MyMeleeType == MeleeType.Mirror)
        {
            _role.BeAttack(Damage, force, condition);
            IsCausedDamage = true;
        }
        else if (MyMeleeType == MeleeType.Block)
        {
            _role.BeAttack(0, force, condition);
        }
        if (AmmoType != ShootAmmoType.Penetration)
            SelfDestroy();
    }
    protected void TriggerAmmo(Ammo _ammo)
    {
        if (MyMeleeType == MeleeType.Melee)
            return;
        Attacker.ReceiveDmg((int)(_ammo.Damage * -(BlockIntensity - 1)));
        if (MyMeleeType == MeleeType.Block || MyMeleeType == MeleeType.Reflect)
        {
            _ammo.SelfDestroy();

        }
        else if (MyMeleeType == MeleeType.Mirror)
        {
            _ammo.ForceReverse();
        }
        if (AmmoType != ShootAmmoType.Penetration)
            SelfDestroy();

    }

}
