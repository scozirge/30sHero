using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyRole : Role
{

    protected const float FrictionDuringTime = 1;
    protected float FrictionDuringTimer = FrictionDuringTime;
    protected bool StartVelocityDecay;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Move()
    {
        FrictionDuringTimeFunc();
        if (StartVelocityDecay)
        {
            MyRigi.velocity *= MoveDecay;
        }
    }
    public override void BeAttack(int _dmg, Vector2 _force, Dictionary<RoleBuffer, BufferData> buffers)
    {
        base.BeAttack(_dmg, _force, buffers);
        StartVelocityDecay = true;
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

    public override void AttackReaction()
    {
        base.AttackReaction();
    }
}
