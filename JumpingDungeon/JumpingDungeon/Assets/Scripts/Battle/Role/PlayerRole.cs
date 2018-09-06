using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    protected int MaxShield;
    float shield;
    protected float Shield
    {
        get { return shield; }
        set
        {
            if (value < 0)
                value = 0;
            else if (value > MaxShield)
                value = MaxShield;
            shield = value;
            ShieldBar.sizeDelta = new Vector2(ShieldRatio * ShieldBarWidth, ShieldBar.rect.height);
        }
    }
    public float ShieldRatio { get { return (float)Shield / (float)MaxShield; } }
    [SerializeField]
    protected RectTransform ShieldBar;
    float ShieldBarWidth;
    [SerializeField]
    float ShieldReGenerateTime;
    float ShieldGenerateNum { get { return MaxShield / ShieldReGenerateTime; } }
    [SerializeField]
    float ShieldRechargeTime;
    bool StartGenerateShield;
    MyTimer ShieldTimer;



    [SerializeField]
    public float MaxAvaterTime;
    public float AvatarTimeRatio { get { return (float)AvatarTimer / (float)MaxAvaterTime; } }
    [SerializeField]
    Text AvatarTimerText;
    [SerializeField]
    protected float AvatarTimeBuff;

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
        ShieldTimer = new MyTimer(ShieldRechargeTime, ShieldRestore, null);
        ShieldBarWidth = ShieldBar.rect.width;
        Shield = MaxShield;
    }
    void RestoreAttack()
    {
        CurAttackState = 0;
    }
    void ShieldRestore()
    {
        StartGenerateShield = true;
    }
    void ShieldGenerate()
    {
        if (Shield < MaxShield)
            if (StartGenerateShield)
                Shield += ShieldGenerateNum * Time.deltaTime;
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
        ShieldTimer.RunTimer();
        ShieldGenerate();
    }
    public override void BeAttack(int _dmg, Vector2 _force, Dictionary<RoleBuffer, BufferData> buffers)
    {
        if (!IsAvatar)
        {
            IsAlive = false;
            SelfDestroy();
        }
        else
        {
            //Damage Shield
            if (_dmg > Shield)
            {
                _dmg = (int)(_dmg - Shield);
                Shield = 0;
            }
            else
            {
                Shield -= _dmg;
                _dmg = 0;
            }
            ShieldTimer.Start(true);
            ShieldTimer.RestartCountDown();
            StartGenerateShield = false;
            base.BeAttack(_dmg, _force, buffers);
        }
    }
    protected void AvatarTimerFunc()
    {
        if (AvatarTimer > 0)
            AvatarTimer -= Time.deltaTime;
        else
        {
            AvatarTimer = 0;
        }
        AvatarTimerText.text = Mathf.Round(AvatarTimer).ToString();
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
                HealHP((int)(MaxHealth * _data.Value + PotionEfficacy));
                break;
            case LootType.InvinciblePotion:
                GetCondition(RoleBuffer.Invicible, _data);
                break;
            case LootType.Money:

                break;
        }
    }



}
