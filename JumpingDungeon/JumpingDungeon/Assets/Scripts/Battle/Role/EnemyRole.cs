using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class EnemyRole : Role
{
    [SerializeField]
    GameObject HealthObj;
    public int ID { get; protected set; }
    public string Name { get; protected set; }
    public int DebutFloor { get; protected set; }
    public EnemyType Type { get; protected set; }
    protected const float FrictionDuringTime = 1;
    protected float FrictionDuringTimer = FrictionDuringTime;
    protected bool StartVelocityDecay;

    AIRoleMove MyAIMove;
    PlayerRole Target;

    public void SetEnemyData(EnemyData _data)
    {
        ID = _data.ID;
        Name = _data.Name;
        DebutFloor = _data.DebutFloor;
        Type = _data.Type;
    }
    protected override void Start()
    {
        base.Start();
        MyAIMove = GetComponent<AIRoleMove>();
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go)
            Target = go.GetComponent<PlayerRole>();
        if (Target && Health <= Target.Damage)
            HealthObj.SetActive(false);
    }
    void SetEnemyDirection()
    {
        if (!Target)
            return;
        Vector2 dir = Target.transform.position - transform.position;

        if (dir.x >= 0)
            DirectX = Direction.Right;
        else
            DirectX = Direction.Left;

        if (dir.y >= 0)
            DirectY = Direction.Top;
        else
            DirectY = Direction.Bottom;
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
        BattleManage.RemoveEnemy(this);
        base.SelfDestroy();
        Drop();
    }

    protected override void Update()
    {
        base.Update();
        SetEnemyDirection();
    }
    public override void AddBuffer(BufferData _buffer)
    {
        base.AddBuffer(_buffer);
        if (_buffer.Type == RoleBuffer.Stun)
            if (MyAIMove)
                MyAIMove.SetCanMove(false);
    }
    public override void RemoveBuffer(BufferData _buffer)
    {
        base.RemoveBuffer(_buffer);
        if (_buffer.Type == RoleBuffer.Stun)
            if (MyAIMove)
                MyAIMove.SetCanMove(false);
    }
    public EnemyRole GetMemberwiseClone()
    {
        EnemyRole role = this.MemberwiseClone() as EnemyRole;
        return role;
    }
}
