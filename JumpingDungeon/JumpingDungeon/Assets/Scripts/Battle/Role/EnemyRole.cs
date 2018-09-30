using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyRole : Role
{


    protected const float FrictionDuringTime = 1;
    protected float FrictionDuringTimer = FrictionDuringTime;
    protected bool StartVelocityDecay;

    AIMove MyAIMove;
    PlayerRole Target;

    protected override void Awake()
    {
        base.Awake();
        MyAIMove = GetComponent<AIMove>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
            Target = go.GetComponent<PlayerRole>();
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
        if (!Target)
            return;
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
    public override void BeAttack(int _dmg, Vector2 _force)
    {
        GetFriction();
        AniPlayer.PlayTrigger("BeAttack", 0);
        base.BeAttack(_dmg, _force);
    }
    void GetFriction()
    {
        StartVelocityDecay = true;
        FrictionDuringTimer = FrictionDuringTime;
    }
    void FrictionDuringTimeFunc()
    {
        if (!StartVelocityDecay)
            return;
        if (FrictionDuringTimer > 0)
            FrictionDuringTimer -= Time.deltaTime;
        else
        {
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
    protected override void AddBuffer(RoleBuffer _buffer, BufferData _data)
    {
        base.AddBuffer(_buffer, _data);
        if (_buffer == RoleBuffer.Stun)
            if (MyAIMove)
                MyAIMove.SetCanMove(false);
    }
    public override void RemoveBuffer(RoleBuffer _buffer)
    {
        base.RemoveBuffer(_buffer);
        if (_buffer == RoleBuffer.Stun)
            if (MyAIMove)
                MyAIMove.SetCanMove(false);
    }
}
