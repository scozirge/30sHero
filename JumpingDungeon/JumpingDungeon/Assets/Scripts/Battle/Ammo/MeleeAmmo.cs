using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAmmo : Ammo
{

    protected bool IsCausedDamage;

    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Vector3 dir = (Vector3)(_dic["Direction"]);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        Launch();
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (IsCausedDamage)
            return;
        base.OnTriggerEnter2D(_col);
        switch (_col.gameObject.tag)
        {
            case "Player":
                PlayerRole pr = _col.GetComponent<PlayerRole>();
                Vector2 force = (pr.transform.position - transform.position).normalized * KnockIntensity;
                Dictionary<RoleBuffer, BufferData> condition = new Dictionary<RoleBuffer, BufferData>();
                condition.Add(RoleBuffer.Stun, new BufferData(StunIntensity, 0));
                pr.BeAttack(Damage, force, condition);
                IsCausedDamage = true;
                break;
            default:
                break;
        }
    }
}
