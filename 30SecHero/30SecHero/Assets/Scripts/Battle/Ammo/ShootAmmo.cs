using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAmmo : Ammo
{
    [SerializeField]
    protected float AmmoSpeed;
    [SerializeField]
    protected float TraceFactor;
    [Tooltip("是否會被牆阻擋")]
    [SerializeField]
    protected bool TriggerWall = true;

    Role Attacker;
    protected Vector3 Ammovelocity;
    public override void Init(Dictionary<string, object> _dic)
    {
        base.Init(_dic);
        Target = ((Role)_dic["Target"]);
        Attacker = (Role)(_dic["Attacker"]);
        Ammovelocity = (Vector3)(_dic["Direction"]) * AmmoSpeed;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, (float)_dic["AmmoRotation"]));
        //transform.LookAt(MyRigi.velocity);

        Launch();
    }
    protected override void OnTriggerEnter2D(Collider2D _col)
    {
        if (!Target)
            return;
        if (AmmoType != ShootAmmoType.LockOnTarget)
            base.OnTriggerStay2D(_col);
        else
            if (GameObject.ReferenceEquals(Target.gameObject, _col.gameObject))
            {
                base.OnTriggerEnter2D(_col);
            }
        if (TriggerWall && _col.tag == "Wall")
        {
            TriggerWallTarget(transform.position);
        }

    }
    protected override void OnTriggerStay2D(Collider2D _col)
    {
        if (!TriggerOnRushRole)
            return;
        if (!Target)
            return;
        if (AmmoType != ShootAmmoType.LockOnTarget)
            base.OnTriggerStay2D(_col);
        else
            if (GameObject.ReferenceEquals(Target.gameObject, _col.gameObject))
            {
                base.OnTriggerStay2D(_col);
            }
    }
    protected virtual void TriggerWallTarget(Vector2 _pos)
    {
        if (HitTargetSound)
            AudioPlayer.PlaySound(HitTargetSound);   
        SpawnDeadParticles(_pos);
        SelfDestroy();
    }
    protected override void TriggerTarget(Role _role, Vector2 _pos)
    {
        if (_role.BuffersExist(RoleBuffer.Untouch))
            return;
        if (!TriggerOnRushRole && _role.OnRush)
            return;
        if (!CheckReadyToDamageTarget(_role))
            return;
        base.TriggerTarget(_role, _pos);
        Vector2 force = (_role.transform.position - transform.position).normalized * KnockIntensity;
        int damage = Value;
        _role.BeAttack(AttackerRoleTag, ref damage, force);
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
            float angle = Mathf.Atan2(MyRigi.velocity.y, MyRigi.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    public override void Launch()
    {
        base.Launch();
        MyRigi.velocity = Ammovelocity;
        float angle = Mathf.Atan2(MyRigi.velocity.y, MyRigi.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
    protected override void SpawnParticles()
    {
        base.SpawnParticles();
        //產生勢力顏色光暈特效
        EffectEmitter.EmitParticle(GameManager.GetForceAmmoParticle(AttackerRoleTag), Vector3.zero, Vector3.zero, transform);
    }
}
