using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerRole : Role
{
    [Tooltip("使用測試模式，非測試模式時，玩家數值為讀表")]
    [SerializeField]
    bool TestMode;
    [LabelOverride("解除變身特效")]
    [Tooltip("解除變身特效")]
    [SerializeField]
    ParticleSystem AvatarRemoveEffect;
    [Tooltip("被攻擊特效(護頓)")]
    [SerializeField]
    ParticleSystem BeHitEffect_Shield;
    [Tooltip("被攻擊特效(沒護頓)")]
    [SerializeField]
    ParticleSystem BeHitEffect;
    [Tooltip("額外動畫播放器")]
    [SerializeField]
    protected AnimationPlayer ExtraAniPlayer;
    public bool IsAvatar { get; protected set; }
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
    [Tooltip("護盾最大值")]
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
    [Tooltip("護盾回復(百分比/秒)")]
    [SerializeField]
    float ShieldGenerateProportion;
    [Tooltip("護盾要沒受到攻擊多久時間(秒)才會開始充能")]
    [SerializeField]
    float ShieldRechargeTime;
    bool StartGenerateShield;
    MyTimer ShieldTimer;
    public override int Damage
    {
        get
        {
            return (int)(BaseDamage *
            (1 + (Buffers.ContainsKey(RoleBuffer.DamageUp) ? Buffers[RoleBuffer.DamageUp].Value : 0 +
            (Buffers.ContainsKey(RoleBuffer.DamageDown) ? -GameSettingData.CurseDamageReduce * (1 - PoisonResistantProportion) : 0))));
        }
    }
    public virtual float MoveSpeed
    {
        get
        {
            return (BaseMoveSpeed + (Buffers.ContainsKey(RoleBuffer.SpeedUp) ? Buffers[RoleBuffer.SpeedUp].Value : 0) + ExtraMoveSpeed) * (1 + (Buffers.ContainsKey(RoleBuffer.Freeze) ?
                -GameSettingData.FreezeMove * (1 - FreezeResistanceProportion) : 0)) + (Buffers.ContainsKey(RoleBuffer.DamageDown) ? DrugAddictionPlus : 0);
        }
    }
    private float extraMoveSpeed;
    public float ExtraMoveSpeed
    {
        get { return extraMoveSpeed; }
        set
        {
            if (value < 0)
                value = 0;
            else if (value > MaxExtraMove)
                value = MaxExtraMove;
            extraMoveSpeed = value;
        }
    }
    [Tooltip("每次殺怪獲得額外速度")]
    [SerializeField]
    float GainMoveFromKilling;
    [Tooltip("當下額外速度衰減完所需時間(秒)")]
    [SerializeField]
    float MoveDepletedTime;
    [Tooltip("殺怪最高額外速度")]
    [SerializeField]
    float MaxExtraMove;
    [Tooltip("變身時間")]
    [SerializeField]
    public float MaxAvaterTime;
    public float AvatarTimeRatio { get { return (float)AvatarTimer / (float)MaxAvaterTime; } }
    [SerializeField]
    Text AvatarTimerText;
    [Tooltip("變身解除無敵時間(秒)")]
    [SerializeField]
    protected float UntochableTime;
    [Tooltip("變身時間加成(秒)")]
    [SerializeField]
    protected float AvatarPotionBuff;
    [Tooltip("技能時間加成(秒)")]
    [SerializeField]
    protected float SkillTimeBuff;
    [Tooltip("技能掉落機率")]
    [SerializeField]
    public float SkillDrop;
    [Tooltip("裝備掉落機率")]
    [SerializeField]
    public int EquipDropWeight;
    [Tooltip("金幣掉落數量追加")]
    [SerializeField]
    protected float GoldDrop;
    [Tooltip("吸血比例")]
    [SerializeField]
    protected float BloodThirsty;
    [Tooltip("藥水效果強化比例")]
    [SerializeField]
    protected float PotionEfficiency;
    [Tooltip("藥水掉落機率")]
    [SerializeField]
    public float PotionDrop;
    const int KeyboardMoveFactor = 1;
    const int CursorMoveFactor = 40;
    [Tooltip("史萊姆跳躍速度")]
    [SerializeField]
    float KeyboardJumpMoveFactor = 4;
    [Tooltip("移動加速特效")]
    [SerializeField]
    ParticleSystem MoveAfterimagePrefab;
    ParticleSystem MoveAfterimage;
    ParticleSystem.MainModule MoveAfterimage_Main;
    int CurAttackState;
    [Tooltip("多久不攻擊會重置攻擊動畫")]
    [SerializeField]
    float DontAttackRestoreTime;
    [Tooltip("自身攻擊時彈開力道")]
    [SerializeField]
    int SelfKnockForce;
    [Tooltip("自身攻擊時彈開暈眩時間")]
    [SerializeField]
    float SelfSturnTime;
    [Tooltip("移動方式")]
    [SerializeField]
    MoveControl ControlDevice;
    [Tooltip("滑鼠在多少距離內不移動")]
    [SerializeField]
    float CursorLimitDist;
    [Tooltip("攝影機")]
    [SerializeField]
    Camera MyCamera;
    [Tooltip("衝刺力道")]
    [SerializeField]
    int RushForce;
    [Tooltip("衝刺音效")]
    [SerializeField]
    AudioClip RushSound;
    [Tooltip("攻擊音效")]
    [SerializeField]
    AudioClip AttackSound;
    [Tooltip("弱化後移動跳躍的CD時間")]
    [SerializeField]
    float JumpCDTime;
    [Tooltip("靜止摩擦力")]
    [SerializeField]
    protected float StopDrag;
    [Tooltip("衝刺無敵時間")]
    [SerializeField]
    protected float RushAntiAmooTime;
    [Tooltip("衝刺CD")]
    [SerializeField]
    protected float RushCD;
    [SerializeField]
    protected Light MyLight;
    [Tooltip("破盾冰風暴")]
    [SerializeField]
    protected Skill Blizzard;
    [Tooltip("破盾冰風暴Prefab")]
    [SerializeField]
    protected MeleeAmmo BlizzardAmmoPrefab;
    [SerializeField]
    PlayerAttack MyAttack;
    [Tooltip("刀光Prefab")]
    [SerializeField]
    Image BladeLight;
    [Tooltip("刀光色調")]
    [SerializeField]
    string BladeLightColor;
    [Tooltip("武器icon")]
    [SerializeField]
    Image[] WeaponIcons;
    ParticleSystem CurBeHitEffect;
    MyTimer AttackTimer;
    MyTimer JumpTimer;
    MyTimer RushTimer;
    MyTimer OnRushTimer;
    [HideInInspector]
    public int FaceLeftOrRight;
    Dictionary<string, Skill> MonsterSkills = new Dictionary<string, Skill>();
    Dictionary<string, Soul> MonsterSouls = new Dictionary<string, Soul>();
    [HideInInspector]
    public EnemyRole ClosestEnemy;
    bool CanJump;
    bool CanRush;

    //附魔
    public float ExtralGoldDropProportion;
    public float NoDamageRecoveryProportion;
    MyTimer NoDamageRecoveryTimer;
    MyTimer NoDamageRecoveryIntervalTimer;
    public float BurningWeaponProportion;
    public float PoisonedWeaponProportion;
    public float FrozenWeaponProportion;
    public float StunningSlashProportion;
    public float AttackRangeProportion;
    public float CarnivorousProportion;
    public float BerserkerProportion;
    public float ReflectShieldProportion;
    public float ConservationOfMassProportion;
    public float ReviveProportion;
    public float ReflectMeleeDamageProportion;
    public float AbsorbElementProportion;
    public float LethalDashProportion;
    bool IsTriggerRevive;//一場戰鬥只會觸發一次復活
    public float InertiaPlus;
    public float ReversalImpactProportion;
    public float DashForLifeProportion;
    public float PharmacistProportion;
    public float AllergyPlus;
    public float DrugFeverProportion;
    int DrugFeverSpeedUp = 50;
    public float AlchemyProportion;
    public float CollectorProportion;
    public float BloodyGoldProportion;
    public float HangOnProportion;
    public float SpeedyJumpProportion;
    public float BreakDoorGoldProportion;
    public float ReAvatarProportion;
    public float TriumphPlus;
    public float ShepherdProportion;
    public float OnFireProportion;
    public float HarvesterProportion;
    public float GhostShelterProportion;
    public float GhostArmorProportion;
    public float FireResistanceProportion;
    public float FireBladeProportion;
    public float FreezeResistanceProportion;
    public float IceArmorProportion;
    public float PoisonResistantProportion;
    public float DrugAddictionPlus;
    public float FortitudeProportion;
    public float NeutralizationProportion;
    public float CowerProportion;
    public float EliteHuntingProportion;
    public struct LastTargeData
    {
        public Role Target;
        public int HitCount;
        public bool Hit(Role _target)
        {
            if (Target && Target.GetInstanceID() == _target.GetInstanceID())
            {
                HitCount++;
                if(HitCount>=3)
                {
                    HitCount = 0;
                    return true;
                }
            }
            else
            {
                Target = _target;
                HitCount = 1;                
            }
            return false;
        }
    }
    public LastTargeData LastTarget;
    public float LightHouseProportion;
    public float TreasureHuntingProportion;
    public float CouragePlus;
    public float KingKillerProportion;
    public float RuleBreakerPlus;
    bool IsTriggerRuleBreaker;//一場戰鬥只會觸發一次無敵
    public float SelfCureProportion;
    public Support SelfCureSkill;
    public Supply SelfCureAmmo;
    public float ChopStrikeProportion;
    public Shoot ChopStrikeSkill;
    public float FireChopProportion;
    public SuicideBombing FireChopSkil;
    public float PoisonChopProportion;
    public SuicideBombing PoisonChopSkil;
    public float FrozenChopProportion;
    public SuicideBombing FrozenChopSkil;
    public float DashImpactProportion;
    public SuicideBombing DashImpactSkil;
    public float SplashThornProportion;
    public Shoot SplashThornSkil;
    


    float BlizzardTime;
    bool CanGenerateBlizzard;

    protected override void Start()
    {
        InitPlayerProperties();
        base.Start();
        if (BlizzardTime > 0)
            CanGenerateBlizzard = true;
        OnRush = false;
        AvatarTimer = MaxAvaterTime;
        AttackTimer = new MyTimer(DontAttackRestoreTime, RestoreAttack, false, false);
        ShieldTimer = new MyTimer(ShieldRechargeTime, ShieldRestore, false, false);
        JumpTimer = new MyTimer(JumpCDTime, SetCanJump, false, false);
        RushTimer = new MyTimer(RushCD, SetCanRush, false, false);
        OnRushTimer = new MyTimer(RushAntiAmooTime, SetNotOnRush, false, false);
        if (NoDamageRecoveryProportion > 0)
        {
            NoDamageRecoveryTimer = new MyTimer(5, SetSelfCure, false, false);
            NoDamageRecoveryIntervalTimer = new MyTimer(1, NoDamageRecovery, true, true);
        }
        ShieldBarWidth = ShieldBar.rect.width;
        Health = MaxHealth;
        Shield = MaxShield;
        InitMoveAfterimage();
        FaceLeftOrRight = 1;
        IsAvatar = true;
        CanJump = true;
        CanRush = true;
        IsTriggerRevive = false;
        IsTriggerRuleBreaker = false;
    }
    void InitPlayerProperties()
    {
        if (TestMode)
            return;
        MaxHealth = (int)Player.GetProperties(RoleProperty.Health);
        BaseDamage = (int)Player.GetProperties(RoleProperty.Strength);
        MaxShield = (int)Player.GetProperties(RoleProperty.Shield);
        ShieldRechargeTime = (float)Player.GetProperties(RoleProperty.ShieldReChargeTime);
        ShieldGenerateProportion = (float)Player.GetProperties(RoleProperty.ShieldRecovery);
        BaseMoveSpeed = (int)Player.GetProperties(RoleProperty.MoveSpeed);
        MaxExtraMove = (int)Player.GetProperties(RoleProperty.MaxMoveSpeed);
        MoveDepletedTime = (float)Player.GetProperties(RoleProperty.MoveDecay);
        MaxAvaterTime = (float)Player.GetProperties(RoleProperty.AvatarTime);
        AvatarPotionBuff = (int)Player.GetProperties(RoleProperty.AvatarPotionBuff);
        SkillTimeBuff = (float)Player.GetProperties(RoleProperty.SkillTimeBuff);
        SkillDrop = (float)Player.GetProperties(RoleProperty.SkillDrop);
        EquipDropWeight = (int)Player.GetProperties(RoleProperty.EquipDrop);
        GoldDrop = (float)Player.GetProperties(RoleProperty.GoldDrop);
        BloodThirsty = (float)Player.GetProperties(RoleProperty.BloodThirsty);
        PotionEfficiency = (float)Player.GetProperties(RoleProperty.PotionEfficiency);
        PotionDrop = (float)Player.GetProperties(RoleProperty.PotionDrop);
        GainMoveFromKilling = (int)Player.GetProperties(RoleProperty.GainMoveFromKilling);

        //附魔
        ExtralGoldDropProportion = Player.GetEnchantProperty(EnchantProperty.ExtralGoldDrop);
        RushCD = (float)Player.GetProperties(RoleProperty.RushCD) - Player.GetEnchantProperty(EnchantProperty.RushCDResuce);
        NoDamageRecoveryProportion = Player.GetEnchantProperty(EnchantProperty.NoDamageRecovery);
        BlizzardTime = Player.GetEnchantProperty(EnchantProperty.ShockWave);
        BurningWeaponProportion = Player.GetEnchantProperty(EnchantProperty.BurningWeapon);
        PoisonedWeaponProportion = Player.GetEnchantProperty(EnchantProperty.PoisonedWeapon);
        FrozenWeaponProportion = Player.GetEnchantProperty(EnchantProperty.FrozenWeapon);
        StunningSlashProportion = Player.GetEnchantProperty(EnchantProperty.StunningSlash);
        AttackRangeProportion = Player.GetEnchantProperty(EnchantProperty.AttackRange);
        CarnivorousProportion = Player.GetEnchantProperty(EnchantProperty.Carnivorous);
        BerserkerProportion = Player.GetEnchantProperty(EnchantProperty.Berserker);
        ConservationOfMassProportion = Player.GetEnchantProperty(EnchantProperty.ConservationOfMass);
        ReflectShieldProportion = Player.GetEnchantProperty(EnchantProperty.ReflectShield);
        ConservationOfMassProportion = Player.GetEnchantProperty(EnchantProperty.ConservationOfMass);
        ReviveProportion = Player.GetEnchantProperty(EnchantProperty.Revive);
        ReflectMeleeDamageProportion = Player.GetEnchantProperty(EnchantProperty.ReflectMeleeDamage);
        AbsorbElementProportion = Player.GetEnchantProperty(EnchantProperty.AbsorbElement);
        LethalDashProportion = Player.GetEnchantProperty(EnchantProperty.LethalDash);
        InertiaPlus = Player.GetEnchantProperty(EnchantProperty.Inertia);
        ReversalImpactProportion = Player.GetEnchantProperty(EnchantProperty.ReversalImpact);
        DashForLifeProportion = Player.GetEnchantProperty(EnchantProperty.DashForLife);
        PharmacistProportion = Player.GetEnchantProperty(EnchantProperty.Pharmacist);
        AllergyPlus = Player.GetEnchantProperty(EnchantProperty.Allergy);
        DrugFeverProportion = Player.GetEnchantProperty(EnchantProperty.DrugFever);
        AlchemyProportion = Player.GetEnchantProperty(EnchantProperty.Alchemy);
        CollectorProportion = Player.GetEnchantProperty(EnchantProperty.Collector);
        BloodyGoldProportion = Player.GetEnchantProperty(EnchantProperty.BloodyGold);
        HangOnProportion = Player.GetEnchantProperty(EnchantProperty.HangOn);
        if (HangOnProportion > 0)
            UntochableTime *= (1 + HangOnProportion);
        SpeedyJumpProportion = Player.GetEnchantProperty(EnchantProperty.SpeedyJump);
        if (SpeedyJumpProportion > 0)
            JumpCDTime *= (1 - SpeedyJumpProportion);
        BreakDoorGoldProportion = Player.GetEnchantProperty(EnchantProperty.BreakDoorGold);
        ReAvatarProportion = Player.GetEnchantProperty(EnchantProperty.ReAvatar);
        TriumphPlus = Player.GetEnchantProperty(EnchantProperty.Triumph);
        ShepherdProportion = Player.GetEnchantProperty(EnchantProperty.Shepherd);
        OnFireProportion = Player.GetEnchantProperty(EnchantProperty.OnFire);
        HarvesterProportion = Player.GetEnchantProperty(EnchantProperty.Harvester);
        GhostShelterProportion = Player.GetEnchantProperty(EnchantProperty.GhostShelter);
        GhostArmorProportion = Player.GetEnchantProperty(EnchantProperty.GhostArmor);
        FireResistanceProportion = Player.GetEnchantProperty(EnchantProperty.FireResistance);
        FireBladeProportion = Player.GetEnchantProperty(EnchantProperty.FireBlade);
        FreezeResistanceProportion = Player.GetEnchantProperty(EnchantProperty.FreezeResistance);
        IceArmorProportion = Player.GetEnchantProperty(EnchantProperty.IceArmor);
        PoisonResistantProportion = Player.GetEnchantProperty(EnchantProperty.PoisonResistant);
        DrugAddictionPlus = Player.GetEnchantProperty(EnchantProperty.DrugAddiction);
        FortitudeProportion = Player.GetEnchantProperty(EnchantProperty.Fortitude);
        NeutralizationProportion = Player.GetEnchantProperty(EnchantProperty.Neutralization);
        CowerProportion = Player.GetEnchantProperty(EnchantProperty.Cower);
        EliteHuntingProportion = Player.GetEnchantProperty(EnchantProperty.EliteHunting);
        LightHouseProportion = Player.GetEnchantProperty(EnchantProperty.LightHouse);
        if (LightHouseProportion > 0)
            MyLight.range = MyLight.range * (1 + LightHouseProportion);
        TreasureHuntingProportion = Player.GetEnchantProperty(EnchantProperty.TreasureHunting);
        CouragePlus = Player.GetEnchantProperty(EnchantProperty.Courage);
        KingKillerProportion = Player.GetEnchantProperty(EnchantProperty.KingKiller);
        RuleBreakerPlus = Player.GetEnchantProperty(EnchantProperty.RuleBreaker);
        SelfCureProportion = Player.GetEnchantProperty(EnchantProperty.SelfCure);
        if (SelfCureProportion>0)
        {
            SelfCureSkill.BehaviorSkill = false;
            SelfCureAmmo.CureProportion = SelfCureProportion;
        }
        ChopStrikeProportion = Player.GetEnchantProperty(EnchantProperty.ChopStrike);
        FireChopProportion = Player.GetEnchantProperty(EnchantProperty.FireChop);
        PoisonChopProportion = Player.GetEnchantProperty(EnchantProperty.PoisonChop);
        FrozenChopProportion = Player.GetEnchantProperty(EnchantProperty.FrozenChop);
        DashImpactProportion = Player.GetEnchantProperty(EnchantProperty.DashImpact);
        SplashThornProportion = Player.GetEnchantProperty(EnchantProperty.SplashThorn);
        SplashThornSkil.DamagePercent = SplashThornProportion;


        if (Player.MyWeapon != null)
            SetEquipIcon(Player.MyWeapon);
        if (AttackRangeProportion > 0)
            MyAttack.SetRange();

        if (BlizzardTime > 0)
        {
            BlizzardAmmoPrefab.SetBuffersTime(BlizzardTime);
        }
    }
    void InitMoveAfterimage()
    {
        MoveAfterimage = EffectEmitter.EmitParticle(MoveAfterimagePrefab, Vector3.zero, Vector3.zero, transform).GetComponentInChildrenExcludeSelf<ParticleSystem>();
        if (MoveAfterimage == null)
            return;
        MoveAfterimage_Main = MoveAfterimage.main;
        MoveAfterimage_Main.maxParticles = 0;
        MoveAfterimage_Main.startLifetime = 0;
    }
    void SetEquipIcon(EquipData _data)
    {
        for (int i = 0; i < WeaponIcons.Length; i++)
        {
            WeaponIcons[i].sprite = _data.Icons[i];
        }
    }
    void SetCanRush()
    {
        CanRush = true;
    }
    void SetNotOnRush()
    {
        OnRush = false;
    }
    void SetCanJump()
    {
        CanJump = true;
    }
    void RestoreAttack()
    {
        CurAttackState = 0;
    }
    void ShieldRestore()
    {
        StartGenerateShield = true;
    }
    protected override void Burn()
    {
        BurningTimer.StartRunTimer = true;
        int damage = (int)(MaxHealth * GameSettingData.BurnDamage);
        damage = (int)(damage * (1 - FireResistanceProportion));
        ReceiveDmg(ref damage);
    }
    void SetSelfCure()
    {
        NoDamageRecoveryIntervalTimer.RestartCountDown();
        NoDamageRecoveryIntervalTimer.StartRunTimer = true;
    }
    void NoDamageRecovery()
    {
        HealHP((int)(MaxHealth * NoDamageRecoveryProportion));
    }
    public void SetBerserkerBladeLight(bool _bool)
    {
        if (_bool)
        {
            Color newCol;
            if (ColorUtility.TryParseHtmlString("#" + BladeLightColor, out newCol))
                BladeLight.color = newCol;
        }
        else
        {
            BladeLight.color = Color.white;
        }
    }
    void ShieldGenerate()
    {
        if (Shield < MaxShield)
            if (StartGenerateShield)
            {
                Shield += ShieldGenerateProportion * MaxShield * Time.deltaTime;
                if (Shield >= MaxShield)
                    CanGenerateBlizzard = true;
            }
    }
    protected void ChangeToStopDrag()
    {
        if (!DragTimer.StartRunTimer)
        {
            MyRigi.drag = StopDrag;
        }
    }
    //重新化身為英雄ReAvatarProportion
    public void ReAvatar()
    {
        IsAvatar = true;
        AvatarTimer = 30;
        AniPlayer.PlayTrigger("Idle", 0);
        EffectEmitter.EmitParticle(AvatarRemoveEffect, Vector3.zero, Vector3.zero, transform);
    }
    public void AttackMotion()
    {
        if (!IsAvatar)
            return;
        //Play Animation
        AttackTimer.StartRunTimer = true;
        CurAttackState++;
        if (CurAttackState == 1)
            AniPlayer.PlayTrigger("Attack1", 0);
        else if (CurAttackState == 2)
            AniPlayer.PlayTrigger("Attack2", 0);
        if (CurAttackState > 1)
            CurAttackState = 0;
        AttackTimer.RestartCountDown();
        CameraController.PlayMotion("Shake1");
        AudioPlayer.PlaySound(AttackSound);
    }
    public override void SelfDestroy()
    {
        base.SelfDestroy();
        BattleManage.BM.CalculateResult();

    }
    public void BumpingAttack()
    {
        if (!IsAvatar)
        {
            int dmg = 99999999;
            ReceiveDmg(ref dmg);
            return;
        }
        ChangeToKnockDrag();
        Vector2 force = MyRigi.velocity.normalized * SelfKnockForce * -1;
        MyRigi.AddForce(force);
        AddBuffer(new BufferData(RoleBuffer.Stun, SelfSturnTime));
    }
    protected override void Update()
    {
        base.Update();
        GameObject go = GameobjectFinder.FindClosestGameobjectWithTag(gameObject, Force.Enemy.ToString());
        if (go)
            ClosestEnemy = go.GetComponent<EnemyRole>();
        AvatarTimerFunc();
        AttackTimer.RunTimer();
        ShieldTimer.RunTimer();
        JumpTimer.RunTimer();
        RushTimer.RunTimer();
        OnRushTimer.RunTimer();
        if (NoDamageRecoveryTimer != null)
        {
            NoDamageRecoveryTimer.RunTimer();
            NoDamageRecoveryIntervalTimer.RunTimer();
        }
        MonsterSkillTimerFunc();
        ShieldGenerate();
        ExtraMoveSpeedDecay();
        SetEnemyDirection();
    }
    void SetEnemyDirection()
    {
        if (!ClosestEnemy)
            return;
        Vector2 dir = ClosestEnemy.transform.position - transform.position;
        if (dir.x >= 0)
            DirectX = Direction.Right;
        else
            DirectX = Direction.Left;

        if (dir.y >= 0)
            DirectY = Direction.Top;
        else
            DirectY = Direction.Bottom;
    }
    public override void BeAttack(Force _attackerForce, ref int _dmg, Vector2 _force)
    {
        if (!IsAvatar)
        {
            Health = 0;
            Shield = 0;
        }
        //寒冰甲(冰凍時減少受到的傷害)
        if (IceArmorProportion > 0 && BuffersExist(RoleBuffer.Freeze))
            _dmg = (int)(_dmg * (1 - IceArmorProportion));
        //堅毅(暈眩時減少受到的傷害)
        if (FortitudeProportion > 0 && BuffersExist(RoleBuffer.Stun))
            _dmg = (int)(_dmg * (1 - FortitudeProportion));
        base.BeAttack(_attackerForce, ref _dmg, _force);
    }
    public override void ReceiveDmg(ref int _dmg)
    {
        base.ReceiveDmg(ref _dmg);
        //受到傷害解除自癒
        if (NoDamageRecoveryTimer != null)
        {
            NoDamageRecoveryTimer.RestartCountDown();
            NoDamageRecoveryTimer.StartRunTimer = true;
            NoDamageRecoveryIntervalTimer.StartRunTimer = false;
        }
    }
    void GenerateBlizzard()//破盾時釋放冰風暴(附魔技能)
    {
        if (!CanGenerateBlizzard)
            return;
        CanGenerateBlizzard = false;
        Blizzard.LaunchAISpell();
    }
    protected override void ShieldBlock(ref int _dmg)
    {
        base.ShieldBlock(ref _dmg);
        if (Shield != 0)
        {
            if (CurBeHitEffect) Destroy(CurBeHitEffect.gameObject);
            if (_dmg > 0)
                if (BeHitEffect_Shield) CurBeHitEffect = EffectEmitter.EmitParticle(BeHitEffect_Shield, Vector2.zero, Vector3.zero, transform);
            //Damage Shield
            if (_dmg >= Shield)
            {
                _dmg = (int)(_dmg - Shield);
                Shield = 0;
                //護盾破碎釋放冰凍衝擊
                GenerateBlizzard();
                //護盾破碎時短暫無敵(一場戰鬥只會觸發一次)
                if (RuleBreakerPlus > 0 && !IsTriggerRuleBreaker)
                {
                    IsTriggerRuleBreaker = true;
                    AddBuffer(RoleBuffer.Immortal, RuleBreakerPlus);
                }
            }
            else
            {
                Shield -= _dmg;
                _dmg = 0;
            }
        }
        else
        {
            if (CurBeHitEffect) Destroy(CurBeHitEffect.gameObject);
            if (BeHitEffect) CurBeHitEffect = EffectEmitter.EmitParticle(BeHitEffect, Vector2.zero, Vector3.zero, transform);
            CameraController.PlayEffect("BeHitFrame");
            CameraController.PlayMotion("Shake1");
        }

        ShieldTimer.StartRunTimer = true;
        ShieldTimer.RestartCountDown();
        StartGenerateShield = false;
    }
    protected void AvatarTimerFunc()
    {
        if (!IsAvatar)
            return;
        if (AvatarTimer > 0)
            AvatarTimer -= Time.deltaTime;
        else//解除變身
        {
            AniPlayer.PlayTrigger("Idle2", 0);
            AvatarTimer = 0;
            ExtraMoveSpeed = 0;
            RemoveAllBuffer();
            AddBuffer(RoleBuffer.Untouch, UntochableTime);
            IsAvatar = false;
            RemoveAllSill();
            //飛濺針刺
            if(SplashThornProportion>0)
                SplashThornSkil.LaunchAISpell();
            EffectEmitter.EmitParticle(AvatarRemoveEffect, Vector3.zero, Vector3.zero, transform);
            if (MoveAfterimage)
            {
                MoveAfterimage_Main.maxParticles = 0;
                MoveAfterimage_Main.startLifetime = 0;
            }
        }
        AvatarTimerText.text = Mathf.Round(AvatarTimer).ToString();
    }

    protected override void Move()
    {
        base.Move();
        //MyRigi.velocity *= MoveDecay;

        if (Buffers.ContainsKey(RoleBuffer.Stun))
            return;

        if (ControlDevice == MoveControl.Cursor)
        {
            //滑鼠移動
            Vector3 cursorPos = Input.mousePosition;
            cursorPos = MyCamera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, 0));
            cursorPos.z = 0;
            float distance = Vector3.Distance(cursorPos, transform.position);
            if (distance > CursorLimitDist)
            {
                Vector3 dir = (cursorPos - transform.position);
                if (dir.x > 0)
                    FaceLeftOrRight = 1;
                else
                    FaceLeftOrRight = -1;
                float origAngle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) * Mathf.Deg2Rad;
                dir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
                Vector2 force = dir * MoveSpeed * CursorMoveFactor;
                MyRigi.velocity = force;
                //衝刺
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 rushForce = dir * RushForce;
                    ChangeToKnockDrag();
                    MyRigi.velocity = rushForce;
                    AudioPlayer.PlaySound(RushSound);
                }
            }
        }
        else if (ControlDevice == MoveControl.Keyboard)//鍵盤移動
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)
                || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                if (!DragTimer.StartRunTimer)
                {
                    DragRecovery();
                    if (IsAvatar)
                    {
                        float xMoveForce = 0;
                        float yMoveForce = 0;
                        xMoveForce = Input.GetAxis("Horizontal") * MoveSpeed * KeyboardMoveFactor;
                        yMoveForce = Input.GetAxis("Vertical") * MoveSpeed * KeyboardMoveFactor;
                        //MyRigi.velocity += new Vector2(xMoveForce, yMoveForce);
                        MyRigi.velocity = new Vector2(xMoveForce, yMoveForce);
                        //衝刺
                        if (CanRush && Input.GetKeyDown(KeyCode.Space))
                        {
                            RushTimer.StartRunTimer = true;
                            OnRushTimer.StartRunTimer = true;
                            //寫少時衝刺CD減少
                            if (DashForLifeProportion > 0)
                            {
                                if (HealthRatio < DashForLifeProportion)
                                    RushTimer.ResetMaxTime(RushCD - 0.5f);
                                else
                                    RushTimer.ResetMaxTime(RushCD);
                            }
                            CanRush = false;
                            OnRush = true;
                            ExtraMoveSpeed += InertiaPlus;
                            Vector2 rushForce;
                            if (xMoveForce == 0 && yMoveForce == 0)
                            {
                                rushForce = new Vector2(FaceLeftOrRight, 0) * RushForce * 1000000;
                            }
                            else
                                rushForce = new Vector2(xMoveForce, yMoveForce) * RushForce * 1000000;
                            ChangeToKnockDrag();
                            MyRigi.velocity = rushForce;
                            //MyRigi.AddForce(rushForce);
                            AudioPlayer.PlaySound(RushSound);
                            //衝刺傷害增加特效
                            if (LethalDashProportion > 0)
                                EffectEmitter.EmitParticle(GameManager.GM.LethalDashParticle, Vector3.zero, Vector3.zero, transform);
                        }
                    }
                    else
                    {
                        if (!CanJump)
                            return;
                        float xMoveForce = 0;
                        float yMoveForce = 0;
                        if (Input.GetAxis("Horizontal") == 0)
                            xMoveForce = 0;
                        else if (Input.GetAxis("Horizontal") > 0)
                            xMoveForce = 1;
                        else if (Input.GetAxis("Horizontal") < 0)
                            xMoveForce = -1;
                        if (Input.GetAxis("Vertical") == 0)
                            yMoveForce = 0;
                        else if (Input.GetAxis("Vertical") > 0)
                            yMoveForce = 1;
                        else if (Input.GetAxis("Vertical") < 0)
                            yMoveForce = -1;
                        if (xMoveForce != 0 || yMoveForce != 0)
                            AniPlayer.PlayTrigger("Jump", 0);
                        xMoveForce *= MoveSpeed * KeyboardJumpMoveFactor;
                        yMoveForce *= MoveSpeed * KeyboardJumpMoveFactor;
                        MyRigi.velocity = new Vector2(xMoveForce, yMoveForce);
                        JumpTimer.StartRunTimer = true;
                        CanJump = false;
                    }
                }
            }
            else
                ChangeToStopDrag();
        }
        FaceTarget();
    }
    void FaceTarget()
    {
        if (ControlDevice == MoveControl.Cursor)
        {
            //滑鼠移動
        }
        else if (ControlDevice == MoveControl.Keyboard)
        {
            //鍵盤移動
            if (Input.GetAxis("Horizontal") == 0) { }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                FaceLeftOrRight = 1;
                if (MoveAfterimage)
                    MoveAfterimage_Main.startRotationY = 0;
            }
            else
            {
                FaceLeftOrRight = -1;
                if (MoveAfterimage)
                    MoveAfterimage_Main.startRotationY = 180;
            }
        }
        Face(FaceLeftOrRight);
    }
    public override void AddBuffer(BufferData _buffer)
    {
        if (MyEnum.CheckEnumExistInArray<RoleBuffer>(ElementalBuff, _buffer.Type))
        {
            //護盾存在時有機率免除負面元素效果
            if (ShieldRatio > 0 && ProbabilityGetter.GetResult(ConservationOfMassProportion))
            {
            }
            else
            {
                //在無護盾狀態下受元素攻擊有機率免除效果並恢復護盾
                if (ShieldRatio <= 0 && ProbabilityGetter.GetResult(AbsorbElementProportion))
                    Shield += MaxShield * 0.3f;
                else
                {
                    //受到負面元素效果時如果自身已經有其他負面元素狀態時，有機率移除所有的負面元素效果
                    if (ProbabilityGetter.GetResult(NeutralizationProportion) && BuffersExistExcept(_buffer.Type,ElementalBuff))
                    {
                        RemoveBufferByType(ElementalBuff);
                        EffectEmitter.EmitParticle(GameManager.GM.PurifyParticle, Vector3.zero, Vector3.zero, transform);
                    }
                    else
                        base.AddBuffer(_buffer);
                }
            }
        }
        else
        {
            base.AddBuffer(_buffer);
        }
    }
    public void Face(int _face)
    {
        if (_face != 1 && _face != -1)
            return;
        RoleTrans.localScale = new Vector3(_face, 1, 1);
    }
    public void GetLoot(LootData _data)
    {
        //煉金術
        if (ProbabilityGetter.GetResult(AlchemyProportion))
        {
            BattleManage.ExtraDropGoldAdd(GameSettingData.GetEnemyDropGold(BattleManage.Floor));
        }
        //喝藥水隨機解除元素
        if (ProbabilityGetter.GetResult(PharmacistProportion))
        {
            List<RoleBuffer> keys = new List<RoleBuffer>(Buffers.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = 0; j < ElementalBuff.Length; j++)
                {
                    if (Buffers[keys[i]].Type == ElementalBuff[j])
                    {
                        RemoveBuffer(Buffers[keys[i]]);
                        i = keys.Count;
                        EffectEmitter.EmitParticle(GameManager.GM.PurifyParticle, Vector3.zero, Vector3.zero, transform);
                        break;
                    }
                }
            }
        }
        //喝藥水加速
        if (ProbabilityGetter.GetResult(DrugFeverProportion))
        {
            ExtraMoveSpeed += DrugFeverSpeedUp;
            EffectEmitter.EmitParticle(GameManager.GM.PotionSpeedUpParticle, Vector3.zero, Vector3.zero, transform);
        }
        switch (_data.Type)
        {
            case LootType.AvataEnergy:
                AvatarTimer += _data.Time * (1 + PotionEfficiency) + AvatarPotionBuff + AllergyPlus;
                break;
            case LootType.DamageUp:
                AddBuffer(RoleBuffer.DamageUp, _data.Time * (1 + PotionEfficiency) + AllergyPlus, _data.Value);
                break;
            case LootType.HPRecovery:
                if (HealthRatio == 1)
                {
                    if (BloodyGoldProportion > 0)
                        BattleManage.ExtraDropGoldAdd((int)(BloodyGoldProportion * (int)(MaxHealth * _data.Value * (1 + PotionEfficiency))));
                }
                HealHP((int)(MaxHealth * _data.Value * (1 + PotionEfficiency)));
                break;
            case LootType.Immortal:
                AddBuffer(RoleBuffer.Immortal, _data.Time * (1 + PotionEfficiency) + AllergyPlus);
                break;
            case LootType.SpeedUp:
                AddBuffer(RoleBuffer.SpeedUp, _data.Time * (1 + PotionEfficiency) + AllergyPlus, _data.Value);
                break;
        }
        AttackMotion();
    }
    public void GetResource(ResourceType _type, int _value)
    {
        switch (_type)
        {
            case ResourceType.Gold:
                BattleManage.EnemyDropGoldAdd((int)(_value + GoldDrop));
                break;
            case ResourceType.Emerald:
                BattleManage.BossDropEmeraldAdd(_value);
                break;
        }
    }
    public void GetEquip(EquipData _data)
    {
        BattleManage.GainEquip(_data);
    }
    public void InitMonsterSkill(string _name, Skill _skill)
    {
        if (!IsAvatar)
            return;
        if (!MonsterSkills.ContainsKey(_name))
        {
            Skill skill = gameObject.AddComponent(_skill.GetType()).CopySkill(_skill);
            skill.PlayerInitSkill();
            MonsterSkills.Add(_name, skill);
        }
    }
    public void GenerateMonsterSkill(string _name, string _spritePath)
    {
        if (!IsAvatar)
            return;
        if (MonsterSkills.ContainsKey(_name))
        {
            MonsterSkills[_name].enabled = true;
            MonsterSkills[_name].PlayerGetSkill(SkillTimeBuff);
            if (!ActiveMonsterSkills.Contains(MonsterSkills[_name]))
                ActiveMonsterSkills.Add(MonsterSkills[_name]);
            if (BuffersExist(RoleBuffer.Freeze))
                MonsterSkills[_name].Freeze(true);
            if (!MonsterSouls.ContainsKey(_name))
            {
                Soul soul = SoulSpawner.SpawnSoul(transform.position);
                soul.Init(this, _spritePath);
                MonsterSouls.Add(_name, soul);
            }
        }
    }
    protected virtual void MonsterSkillTimerFunc()
    {
        for (int i = 0; i < ActiveMonsterSkills.Count; i++)
        {
            ActiveMonsterSkills[i].PSkillTimer -= Time.deltaTime;
            if (ActiveMonsterSkills[i].PSkillTimer <= 0)
            {
                if (MonsterSouls.ContainsKey(ActiveMonsterSkills[i].PSkillName))
                {
                    MonsterSouls[ActiveMonsterSkills[i].PSkillName].SelfDestroy();
                    MonsterSouls.Remove(ActiveMonsterSkills[i].PSkillName);
                }
                if (MonsterSkills.ContainsKey(ActiveMonsterSkills[i].name))
                    MonsterSkills.Remove(ActiveMonsterSkills[i].PSkillName);
                ActiveMonsterSkills[i].InactivePlayerSkill();
                ActiveMonsterSkills.RemoveAt(i);
            }
        }
    }
    public void GetExtraMoveSpeed()
    {
        if (!IsAvatar)
            return;
        ExtraMoveSpeed += GainMoveFromKilling;
        ExtraAniPlayer.PlayTrigger("SpeedUp", 0);
    }

    void ExtraMoveSpeedDecay()
    {
        if (!MoveAfterimage)
            return;
        if (!IsAvatar)
            return;
        if (Buffers.ContainsKey(RoleBuffer.Stun))
        {

            MoveAfterimage_Main.maxParticles = 0;
            MoveAfterimage_Main.startLifetime = 0;
            return;
        }
        if (ExtraMoveSpeed > 0)
        {
            float decay = (ExtraMoveSpeed / MoveDepletedTime);
            if (decay < 1)
                decay = 1;
            ExtraMoveSpeed -= Time.deltaTime * decay;
            int particleCount = 10 + Mathf.RoundToInt(ExtraMoveSpeed / 5);
            if (particleCount > 40)
                particleCount = 40;
            MoveAfterimage_Main.maxParticles = particleCount;
            float lifeTime = ExtraMoveSpeed / 100;
            if (lifeTime > 0.5)
                lifeTime = 0.5f;
            MoveAfterimage_Main.startLifetime = lifeTime;
        }
    }
    public void HealFromCauseDamage(int _damage)
    {
        if (BloodThirsty <= 0)
            return;
        HealHP((int)(_damage * (1 + BloodThirsty)));
        //Debug.Log("Damage=" + Damage + ",True Damage=" + _damage + ", Vampire=" + (int)(_damage * (BloodThirsty)));
    }
    public override void RemoveAllSill()
    {
        base.RemoveAllSill();
        List<string> keys = new List<string>(MonsterSouls.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            MonsterSouls[keys[i]].SelfDestroy();
        }
        MonsterSouls = new Dictionary<string, Soul>();
    }
    protected override bool DeathCheck()
    {
        if (!IsTriggerRevive && Health <= 0)
        {
            if (ProbabilityGetter.GetResult(ReviveProportion))
            {
                Health = 1;
                AddBuffer(RoleBuffer.Untouch, 1);
                Shield = MaxShield * 0.5f;
                IsTriggerRevive = true;
            }
        }
        bool death = base.DeathCheck();
        if (death)
        {
            Vector3 pos = MyLight.transform.position;
            MyLight.transform.SetParent(BattleManage.BM.transform);
            MyLight.transform.position = pos;
            MyLight.enabled = true;
        }
        return death;
    }
    public void AddAvarTime(float _time)
    {
        if (!IsAvatar)
            return;
        AvatarTimer += _time;
    }
}
