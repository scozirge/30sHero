using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAmmo : Ammo
{
    [SerializeField]
    ShootAmmoType AmmoType;
    [SerializeField]
    protected float AmmoSpeed;
    [SerializeField]
    protected float TraceFactor;


    protected bool IsCausedDamage;
    protected Vector3 Ammovelocity;
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Ammovelocity = (Vector3)(_dic["Direction"]) * AmmoSpeed;
        //transform.LookAt(MyRigi.velocity);

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
                Vector2 force = (pr.transform.position - transform.position).normalized * KnockForce;
                Dictionary<RoleBuffer, BufferData> condition = new Dictionary<RoleBuffer, BufferData>();
                condition.Add(RoleBuffer.Stun, new BufferData(StunDuration, 0));
                pr.BeAttack(Damage, force, condition);
                if (AmmoType!=ShootAmmoType.Penetration)
                    SelfDestroy();
                IsCausedDamage = true;
                break;
            default:
                break;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (TraceFactor > 0)
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
