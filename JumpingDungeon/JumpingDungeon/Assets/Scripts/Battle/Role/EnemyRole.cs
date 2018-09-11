using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyRole : Role
{

    protected const float FrictionDuringTime = 1;
    protected float FrictionDuringTimer = FrictionDuringTime;
    protected bool StartVelocityDecay;
    PlayerRole Target;

    protected override void Awake()
    {
        base.Awake();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRole>();
    }
    protected override void Move()
    {
        FrictionDuringTimeFunc();
        if (StartVelocityDecay)
        {
            MyRigi.velocity *= MoveDecay;
        }
        FaceTarget();
    }
    void FaceTarget()
    {
        if (transform.position.x > Target.transform.position.x)
        {
            RoleTrans.localScale = Vector2.one;
        }
        else
        {
            RoleTrans.localScale = new Vector2(-1, 1);
        }
    }
    public override void Attack()
    {
        base.Attack();
        AniPlayer.PlayTrigger("Attack", 0);
    }
    public override void PreAttack()
    {
        base.PreAttack();
        AniPlayer.PlayTrigger_NoPlayback("PreAttack", 0);
    }
    public override void BeAttack(int _dmg, Vector2 _force, Dictionary<RoleBuffer, BufferData> buffers)
    {
        base.BeAttack(_dmg, _force, buffers);
        StartVelocityDecay = true;
        AniPlayer.PlayTrigger("BeAttack", 0);
    }
    void FrictionDuringTimeFunc()
    {
        if (!StartVelocityDecay)
            return;
        if (FrictionDuringTimer > 0)
            FrictionDuringTimer -= Time.deltaTime;
        else
        {
            FrictionDuringTimer = FrictionDuringTime;
            StartVelocityDecay = false;
        }
    }
    protected override void SelfDestroy()
    {
        base.SelfDestroy();
        Drop();
    }

    protected override void Update()
    {
        base.Update();
    }
}
