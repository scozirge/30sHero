using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : Ammo
{
    [SerializeField]
    protected float AmmoSpeed;
    [SerializeField]
    protected float TraceFactor;

    protected Vector3 Ammovelocity;
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Target = ((Role)_dic["Target"]);
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
        if (Value > 0)
            _role.HealHP(Value);
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
}
