using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAmmo : Ammo
{


    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Vector3 dir = (Vector3)(_dic["Direction"]);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Launch();
    }
    protected override void OnTriggerStay2D(Collider2D _col)
    {
        if (IsCausedDamage && AmmoType != ShootAmmoType.Penetration)
            return;
        base.OnTriggerStay2D(_col);
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (IsCausedDamage && AmmoType != ShootAmmoType.Penetration)
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
        IsCausedDamage = true;
        if (AmmoType != ShootAmmoType.Penetration)
            SelfDestroy();
    }
}
