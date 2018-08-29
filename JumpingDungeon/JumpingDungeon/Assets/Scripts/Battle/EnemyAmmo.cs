using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmo : Ammo
{

    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Launch();
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        base.OnTriggerEnter2D(_col);
        switch (_col.gameObject.tag)
        {
            case "Player":
                PlayerRole pr = _col.GetComponent<PlayerRole>();
                Vector2 force = (pr.transform.position - transform.position).normalized * 100000;
                pr.BeAttack(Damage, force);
                SelfDestroy();
                break;
            default:
                break;
        }
    }
    public override void Launch()
    {
        base.Launch();
    }
}
