using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAmmo : Ammo
{
    [SerializeField]
    protected float AmmoSpeed;
    [SerializeField]
    protected float TraceFactor;


    Role Attacker;
    protected Vector3 Ammovelocity;
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Target = ((Role)_dic["Target"]);
        Attacker = (Role)(_dic["Attacker"]);
        Ammovelocity = (Vector3)(_dic["Direction"]) * AmmoSpeed;
        //transform.LookAt(MyRigi.velocity);

        Launch();
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (!Target)
            return;
        if (IsCausedDamage && AmmoType != ShootAmmoType.Permanent)
            return;
        if (AmmoType != ShootAmmoType.LockOnTarget)
            base.OnTriggerStay2D(_col);
        else
            if (GameObject.ReferenceEquals(Target.gameObject, _col.gameObject))
            {
                base.OnTriggerEnter2D(_col);
            }

    }
    protected override void OnTriggerStay2D(Collider2D _col)
    {
        if (!Target)
            return;
        if (IsCausedDamage && AmmoType != ShootAmmoType.Permanent)
            return;
        if (AmmoType != ShootAmmoType.LockOnTarget)
            base.OnTriggerStay2D(_col);
        else
            if (GameObject.ReferenceEquals(Target.gameObject, _col.gameObject))
            {
                base.OnTriggerStay2D(_col);
            }
    }
    protected override void TriggerTarget(Role _role)
    {
        base.TriggerTarget(_role);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        _role.BeAttack(Value, force);
        if (AmmoType != ShootAmmoType.Permanent)
            SelfDestroy();
    }
    protected override void Update()
    {
        base.Update();
        if (TraceFactor > 0 && Target)
        {
            Vector2 targetVel = (Target.transform.position - transform.position).normalized * AmmoSpeed;
            MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, targetVel, TraceFactor);
        }
    }
    public override void Launch()
    {
        base.Launch();
        MyRigi.velocity = Ammovelocity;
    }
    public override void ForceReverse()
    {
        base.ForceReverse();
        if (AttackerRoleTag == Force.Player)
        {
            AttackerRoleTag = Force.Enemy;
            tag = AmmoForce.EnemyAmmo.ToString();
            TargetRoleTag = Force.Player;
        }
        else
        {
            AttackerRoleTag = Force.Player;
            tag = AmmoForce.PlayerAmmo.ToString();
            TargetRoleTag = Force.Enemy;
        }
        Target = Attacker;
        MyRigi.velocity *= -1;
    }
}
