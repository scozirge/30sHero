using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerRole : Role
{
    public bool IsAvatar
    {
        get
        {
            if (AvatarTimer > 0)
                return true;
            else
                return false;
        }
    }
    private float avatarTimer;
    public float AvatarTimer
    {
        get { return avatarTimer; }
        set
        {
            if (value < 0)
                value = 0;
            else if (value > MaxAvaterTime)
                value = MaxAvaterTime;
            avatarTimer = value;
        }
    }
    [SerializeField]
    public float MaxAvaterTime;
    public float AvatarTimeRatio { get { return (float)AvatarTimer / (float)MaxAvaterTime; } }
    [SerializeField]
    protected float AvatarTimeBuff;
    [SerializeField]
    protected RectTransform AvatarTimeBar;
    public virtual int EnergyDrop { get { return BaseEnergyDrop + ExtraEnergyDrop; } }
    public int ExtraEnergyDrop { get; protected set; }
    [SerializeField]
    protected int BaseEnergyDrop;
    public virtual int MoneyDrop { get { return BaseMoneyDrop + ExtraMoneyDrop; } }
    public int ExtraMoneyDrop { get; protected set; }
    [SerializeField]
    protected int BaseMoneyDrop;
    public virtual int Bloodthirsty { get { return BaseBloodthirsty + ExtraBloodthirsty; } }
    public int ExtraBloodthirsty { get; protected set; }
    [SerializeField]
    protected int BaseBloodthirsty;
    public virtual int PotionEfficacy { get { return BasePotionEfficacy + ExtraPotionEfficacy; } }
    public int ExtraPotionEfficacy { get; protected set; }
    [SerializeField]
    protected int BasePotionEfficacy;
    const int MoveFactor = 1;

    int CurAttackState;
    [SerializeField]
    float DontAttackRestoreTime;
    MyTimer AttackTimer;


    protected override void Awake()
    {
        base.Awake();
        AvatarTimer = MaxAvaterTime;
        AttackTimer = new MyTimer(DontAttackRestoreTime, RestoreAttack, null);
    }
    void RestoreAttack()
    {
        CurAttackState = 0;
    }
    public override void Attack()
    {
        base.Attack();
        Vector2 force = MyRigi.velocity.normalized * 100000 * -1;
        MyRigi.AddForce(force);
        GetCondition(RoleBuffer.Stun, new BufferData(0.3f, 0));

        //Play Animation
        AttackTimer.Start(true);
        CurAttackState++;
        if (CurAttackState == 1)
            AniPlayer.PlayTrigger("Attack1", 0);
        else if (CurAttackState == 2)
            AniPlayer.PlayTrigger("Attack2", 0);
        if (CurAttackState > 1)
            CurAttackState = 0;
        AttackTimer.RestartCountDown();
    }
    protected override void Update()
    {
        base.Update();
        AvatarTimerFunc();
        AttackTimer.RunTimer();
    }
    public override void BeAttack(int _dmg, Vector2 _force, Dictionary<RoleBuffer, BufferData> buffers)
    {
        if (!IsAvatar)
        {
            IsAlive = false;
            SelfDestroy();
        }
        else
            base.BeAttack(_dmg, _force, buffers);
    }
    protected void AvatarTimerFunc()
    {
        if (AvatarTimer > 0)
            AvatarTimer -= Time.deltaTime;
        else
        {
            AvatarTimer = 0;
        }
        AvatarTimeBar.localScale = new Vector2(AvatarTimeRatio, 1);
    }
    protected override void Move()
    {
        base.Move();
        if (Buffers.ContainsKey(RoleBuffer.Stun))
            return;
        float xMoveForce = Input.GetAxis("Horizontal") * MoveSpeed * MoveFactor;
        float yMoveForce = Input.GetAxis("Vertical") * MoveSpeed * MoveFactor;
        MyRigi.velocity += new Vector2(xMoveForce, yMoveForce);
    }
    public void GetLoot(LootType _type, BufferData _data)
    {
        switch (_type)
        {
            case LootType.AvataEnergy:
                AvatarTimer += _data.Time * (1 + AvatarTimeBuff);
                break;
            case LootType.DamageBuff:
                GetCondition(RoleBuffer.DamageBuff, _data);
                break;
            case LootType.Euipment:

                break;
            case LootType.HPRecovery:
                HealHP((int)((1 + PotionEfficacy) * _data.Value));
                break;
            case LootType.InvinciblePotion:
                GetCondition(RoleBuffer.Invicible, _data);
                break;
            case LootType.Money:

                break;
        }
    }



}
