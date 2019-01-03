using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerRole : Role
{
    [Tooltip("使用測試模式，非測試模式時，玩家數值為讀表")]
    [SerializeField]
    bool TestMode;
    [LabelOverride("衝刺特效")]
    [SerializeField]
    ParticleSystem RushEffect;
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
            (Buffers.ContainsKey(RoleBuffer.DamageDown) ? -GameSettingData.CurseDamageReduce * (1 - MyEnchant[EnchantProperty.PoisonResistant]) : 0))));
        }
    }
    public virtual float MoveSpeed
    {
        get
        {
            return (BaseMoveSpeed + (Buffers.ContainsKey(RoleBuffer.SpeedUp) ? Buffers[RoleBuffer.SpeedUp].Value : 0) + ExtraMoveSpeed) * (1 + (Buffers.ContainsKey(RoleBuffer.Freeze) ?
                -GameSettingData.FreezeMove * (1 - MyEnchant[EnchantProperty.FreezeResistance]) : 0)) + (Buffers.ContainsKey(RoleBuffer.DamageDown) ? MyEnchant[EnchantProperty.DrugAddiction] : 0);
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
    [SerializeField]
    TextTexture MyAvatarText;
    [SerializeField]
    Animator AvatarTimerAni;
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
    [LabelOverride("變回史萊姆音效")]
    [SerializeField]
    AudioClip RemoveAvatarSound;
    [LabelOverride("變身時間倒數音效")]
    [SerializeField]
    AudioClip CountdownSound;
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
    public Dictionary<EnchantProperty, float> MyEnchant = new Dictionary<EnchantProperty, float>();
    List<Skill> MyEnchantSkill = new List<Skill>();


    MyTimer NoDamageRecoveryTimer;
    MyTimer NoDamageRecoveryIntervalTimer;
    bool IsTriggerRevive;//一場戰鬥只會觸發一次復活
    int DrugFeverSpeedUp = 50;
    public struct LastTargeData
    {
        public Role Target;
        public int HitCount;
        public bool Hit(Role _target)
        {
            if (Target && Target.GetInstanceID() == _target.GetInstanceID())
            {
                HitCount++;
                if (HitCount >= 3)
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
    bool IsTriggerRuleBreaker;//一場戰鬥只會觸發一次無敵
    public Support SelfCureSkill;
    public Supply SelfCureAmmo;
    public Shoot ChopStrikeSkill;
    public SuicideBombing FireChopSkil;
    public SuicideBombing PoisonChopSkil;
    public SuicideBombing FrozenChopSkil;
    public SuicideBombing DashImpactSkil;
    public Shoot SplashThornSkil;
    public SuicideBombing OverloadSkil;
    int FuryBeAttackTImes;
    public SuicideBombing FurySkil;
    MyTimer FuryTimer;
    public Shoot CrossNailSkill;
    public Shoot SplashNailSkill;
    public Shoot ShurikenSkill;
    public Shoot BatSkill;
    public Shoot FrozenBallSkill;
    public Shoot ShockBallSkill;
    public Shoot PoisonedBallSkill;
    public Shoot FireBallSkill;
    public SuicideBombing FireArmorSkill;
    public SuicideBombing FrozenArmorSkill;
    public SuicideBombing PoisonedArmorSkill;
    public SuicideBombing WindArmorSkill;
    bool CanGenerateShockWave;


    public void ShowMyEnchantInfo()
    {
        Debug.Log("顯示戰鬥中附魔資訊");
        List<EnchantProperty> keys = new List<EnchantProperty>(MyEnchant.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (MyEnchant[keys[i]] != 0)
                Debug.Log(keys[i] + "=" + MyEnchant[keys[i]]);
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        if (IsAvatar)
            AniPlayer.PlayTrigger("Idle1", 0);
        else
            AniPlayer.PlayTrigger("Idle2", 0);
    }
    protected override void Awake()
    {
        base.Awake();
        IsAvatar = true;
    }
    protected override void Start()
    {
        InitPlayerProperties();
        base.Start();
        if (MyEnchant[EnchantProperty.ShockWave] > 0)
            CanGenerateShockWave = true;
        OnRush = false;
        AvatarTimer = MaxAvaterTime;
        AttackTimer = new MyTimer(DontAttackRestoreTime, RestoreAttack, false, false);
        ShieldTimer = new MyTimer(ShieldRechargeTime, ShieldRestore, false, false);
        JumpTimer = new MyTimer(JumpCDTime, SetCanJump, false, false);
        RushTimer = new MyTimer(RushCD, SetCanRush, false, false);
        OnRushTimer = new MyTimer(RushAntiAmooTime, SetNotOnRush, false, false);
        Skill[] skills = GetComponents<Skill>();
        for (int i = 0; i < skills.Length; i++)
        {
            MyEnchantSkill.Add(skills[i]);
        }
        if (MyEnchant[EnchantProperty.NoDamageRecovery] > 0)
        {
            NoDamageRecoveryTimer = new MyTimer(5, SetSelfCure, false, false);
            NoDamageRecoveryIntervalTimer = new MyTimer(1, NoDamageRecovery, true, true);
        }
        ShieldBarWidth = ShieldBar.rect.width;
        Health = MaxHealth;
        Shield = MaxShield;
        InitMoveAfterimage();
        FaceLeftOrRight = 1;
        CanJump = true;
        CanRush = true;
        IsTriggerRevive = false;
        IsTriggerRuleBreaker = false;
    }
    void InitPlayerProperties()
    {
        //附魔
        for (int i = 0; i < MyEnum.GetTypeCount<EnchantProperty>(); i++)
        {
            MyEnchant.Add((EnchantProperty)i, Player.GetEnchantProperty((EnchantProperty)i));
        }
        if (TestMode)
        {
            return;
        }

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


        //武器紙娃娃
        if (Player.MyWeapon != null)
            SetEquipIcon(Player.MyWeapon);
        else
            SetEquipIcon(WeaponData.GetDefaultWeapon());


        RushCD = (float)Player.GetProperties(RoleProperty.RushCD) - MyEnchant[EnchantProperty.RushCDResuce];
        UntochableTime *= (1 + MyEnchant[EnchantProperty.HangOn]);
        JumpCDTime *= (1 - MyEnchant[EnchantProperty.SpeedyJump]);
        MyLight.range = MyLight.range * (1 + MyEnchant[EnchantProperty.LightHouse]);
        if (MyEnchant[EnchantProperty.SelfCure] > 0)
        {
            SelfCureSkill.BehaviorSkill = false;
            SelfCureAmmo.CureProportion = MyEnchant[EnchantProperty.SelfCure];
        }
        SplashThornSkil.DamagePercent = MyEnchant[EnchantProperty.SplashThorn];
        if (MyEnchant[EnchantProperty.Fury] > 0)
            FuryTimer = new MyTimer(MyEnchant[EnchantProperty.Fury], FuryTimeUp, false, false);
        if (MyEnchant[EnchantProperty.CrossNail] > 0)
        {
            CrossNailSkill.DamagePercent = MyEnchant[EnchantProperty.CrossNail];
            CrossNailSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.SplashNail] > 0)
        {
            SplashNailSkill.DamagePercent = MyEnchant[EnchantProperty.SplashNail];
            SplashNailSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.Shuriken] > 0)
        {
            ShurikenSkill.DamagePercent = MyEnchant[EnchantProperty.Shuriken];
            ShurikenSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.Bat] > 0)
        {
            BatSkill.DamagePercent = MyEnchant[EnchantProperty.Bat];
            BatSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.FrozenBall] > 0)
        {
            FrozenBallSkill.DamagePercent = MyEnchant[EnchantProperty.FrozenBall];
            FrozenBallSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.ShockBall] > 0)
        {
            ShockBallSkill.DamagePercent = MyEnchant[EnchantProperty.ShockBall];
            ShockBallSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.PoisonedBall] > 0)
        {
            PoisonedBallSkill.DamagePercent = MyEnchant[EnchantProperty.PoisonedBall];
            PoisonedBallSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.FireBall] > 0)
        {
            FireBallSkill.DamagePercent = MyEnchant[EnchantProperty.FireBall];
            FireBallSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.FireArmor] > 0)
        {
            FireArmorSkill.DamagePercent = MyEnchant[EnchantProperty.FireArmor];
            FireArmorSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.FrozenArmor] > 0)
        {
            FrozenArmorSkill.DamagePercent = MyEnchant[EnchantProperty.FrozenArmor];
            FrozenArmorSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.PoisonedArmor] > 0)
        {
            PoisonedArmorSkill.DamagePercent = MyEnchant[EnchantProperty.PoisonedArmor];
            PoisonedArmorSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.WindArmor] > 0)
        {
            WindArmorSkill.DamagePercent = MyEnchant[EnchantProperty.WindArmor];
            WindArmorSkill.BehaviorSkill = false;
        }
        if (MyEnchant[EnchantProperty.AttackRange] > 0)
            MyAttack.SetRange();

        if (MyEnchant[EnchantProperty.ShockWave] > 0)
        {
            BlizzardAmmoPrefab.SetBuffersTime(MyEnchant[EnchantProperty.ShockWave]);
        }



    }
    void FuryTimeUp()
    {
        FuryBeAttackTImes = 0;
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
        damage = (int)(damage * (1 - MyEnchant[EnchantProperty.FireResistance]));
        ReceiveDmg(ref damage);
    }
    void SetSelfCure()
    {
        NoDamageRecoveryIntervalTimer.RestartCountDown();
        NoDamageRecoveryIntervalTimer.StartRunTimer = true;
    }
    void NoDamageRecovery()
    {
        HealHP((int)(MaxHealth * MyEnchant[EnchantProperty.NoDamageRecovery]));
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
                if (Shield >= MaxShield && MyEnchant[EnchantProperty.ShockWave] > 0)
                    CanGenerateShockWave = true;
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
        ActiveEnchantSkill(true);
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
        if (MyEnchant[EnchantProperty.Fury] > 0)
            FuryTimer.RunTimer();
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
        if (MyEnchant[EnchantProperty.IceArmor] > 0 && BuffersExist(RoleBuffer.Freeze))
            _dmg = (int)(_dmg * (1 - MyEnchant[EnchantProperty.IceArmor]));
        //堅毅(暈眩時減少受到的傷害)
        if (MyEnchant[EnchantProperty.Fortitude] > 0 && BuffersExist(RoleBuffer.Stun))
            _dmg = (int)(_dmg * (1 - MyEnchant[EnchantProperty.Fortitude]));
        base.BeAttack(_attackerForce, ref _dmg, _force);
        if (_dmg > 0)
        {
            //被攻擊觸發累積怒火次數，時間內滿3次發動怒火沖天
            if (MyEnchant[EnchantProperty.Fury] > 0)
            {
                FuryTimer.StartRunTimer = true;
                FuryBeAttackTImes++;
                if (FuryBeAttackTImes >= 3)
                {
                    FuryBeAttackTImes = 0;
                    FurySkil.LaunchAISpell();
                }
                else
                {
                    FuryTimer.RestartCountDown();
                }
            }
            //被攻擊觸發火焰新星
            if (MyEnchant[EnchantProperty.FireArmor] > 0 && ProbabilityGetter.GetResult(0.2f))
            {
                FireArmorSkill.LaunchAISpell();
            }
            //被攻擊觸發寒霜新星
            if (MyEnchant[EnchantProperty.FrozenArmor] > 0 && ProbabilityGetter.GetResult(0.2f))
            {
                FrozenArmorSkill.LaunchAISpell();
            }
            //被攻擊觸發劇毒新星
            if (MyEnchant[EnchantProperty.PoisonedArmor] > 0 && ProbabilityGetter.GetResult(0.2f))
            {
                PoisonedArmorSkill.LaunchAISpell();
            }
            //被攻擊觸發颶風新星
            if (MyEnchant[EnchantProperty.WindArmor] > 0 && ProbabilityGetter.GetResult(0.2f))
            {
                WindArmorSkill.LaunchAISpell();
            }
        }
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
        if (!CanGenerateShockWave)
            return;
        CanGenerateShockWave = false;
        Blizzard.LaunchAISpell();
    }
    protected override void ShieldBlock(ref int _dmg)
    {
        base.ShieldBlock(ref _dmg);
        if (Shield != 0)
        {
            if (CurBeHitEffect) Destroy(CurBeHitEffect.gameObject);
            if (_dmg > 0)
                if (BeHitEffect_Shield) CurBeHitEffect = EffectEmitter.EmitParticle(BeHitEffect_Shield, Vector2.zero, Vector3.zero, transform).MyParticle;
            //Damage Shield
            if (_dmg >= Shield)
            {
                _dmg = (int)(_dmg - Shield);
                Shield = 0;
                //護盾破碎釋放冰凍衝擊
                GenerateBlizzard();
                //護盾破碎時短暫無敵(一場戰鬥只會觸發一次)
                if (MyEnchant[EnchantProperty.RuleBreaker] > 0 && !IsTriggerRuleBreaker)
                {
                    IsTriggerRuleBreaker = true;
                    AddBuffer(RoleBuffer.Immortal, MyEnchant[EnchantProperty.RuleBreaker]);
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
            if (BeHitEffect) CurBeHitEffect = EffectEmitter.EmitParticle(BeHitEffect, Vector2.zero, Vector3.zero, transform).MyParticle;
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
            AudioPlayer.PlaySound(RemoveAvatarSound);
            AniPlayer.PlayTrigger("Idle2", 0);
            AvatarTimer = 0;
            ExtraMoveSpeed = 0;
            RemoveAllBuffer();
            AddBuffer(RoleBuffer.Untouch, UntochableTime);
            IsAvatar = false;
            RemoveAllSill();
            //飛濺針刺
            if (MyEnchant[EnchantProperty.SplashThorn] > 0)
                SplashThornSkil.LaunchAISpell();
            EffectEmitter.EmitParticle(AvatarRemoveEffect, Vector3.zero, Vector3.zero, transform);
            if (MoveAfterimage)
            {
                MoveAfterimage_Main.maxParticles = 0;
                MoveAfterimage_Main.startLifetime = 0;
            }
            ActiveEnchantSkill(false);
        }
        if (MyAvatarText.Number != Mathf.Round(AvatarTimer))
        {
            MyAvatarText.SetNumber((int)Mathf.Round(AvatarTimer));
            AudioPlayer.PlaySound(CountdownSound);
            if (AvatarTimerAni != null)
                if (AvatarTimer <= 10)
                    AvatarTimerAni.SetTrigger("TimeAlarm");
        }
        /*
        if (AvatarTimerText.text != Mathf.Round(AvatarTimer).ToString())
        {
            AvatarTimerText.text = Mathf.Round(AvatarTimer).ToString();
            if (AvatarTimerAni != null)
                if (AvatarTimer <= 10)
                    AvatarTimerAni.SetTrigger("TimeAlarm");
        }
        */
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
                            if (MyEnchant[EnchantProperty.DashForLife] > 0)
                            {
                                if (HealthRatio < MyEnchant[EnchantProperty.DashForLife])
                                    RushTimer.ResetMaxTime(RushCD - 0.5f);
                                else
                                    RushTimer.ResetMaxTime(RushCD);
                            }
                            CanRush = false;
                            OnRush = true;
                            ExtraMoveSpeed += MyEnchant[EnchantProperty.Inertia];
                            Vector2 rushForce;
                            if (xMoveForce == 0 && yMoveForce == 0)
                            {
                                rushForce = new Vector2(FaceLeftOrRight, 0) * RushForce;
                            }
                            else
                                rushForce = new Vector2(xMoveForce, yMoveForce) * RushForce;
                            ChangeToKnockDrag();
                            MyRigi.velocity = rushForce;
                            //MyRigi.AddForce(rushForce);
                            AudioPlayer.PlaySound(RushSound);
                            //衝刺特效RushEffect
                            float angle = Mathf.Atan2(MyRigi.velocity.y, MyRigi.velocity.x) * Mathf.Rad2Deg;
                            EffectEmitter.EmitParticle(RushEffect, Vector3.zero, new Vector3(0, 0, angle), transform);
                            //衝刺傷害增加特效  
                            if (MyEnchant[EnchantProperty.LethalDash] > 0)
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
        if (BuffersExist(RoleBuffer.Immortal))
        {
            if (MyEnum.CheckEnumExistInArray<RoleBuffer>(ElementalBuff, _buffer.Type))
            {
                return;
            }
        }
        if (MyEnum.CheckEnumExistInArray<RoleBuffer>(ElementalBuff, _buffer.Type))
        {
            //護盾存在時有機率免除負面元素效果
            if (ShieldRatio > 0 && ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.ConservationOfMass]))
            {
            }
            else
            {
                //在無護盾狀態下受元素攻擊有機率免除效果並恢復護盾
                if (ShieldRatio <= 0 && ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.AbsorbElement]))
                    Shield += MaxShield * 0.3f;
                else
                {
                    bool addBuff = true;
                    //受到負面元素效果時如果自身已經有其他負面元素狀態時
                    if (BuffersExistExcept(_buffer.Type, ElementalBuff))
                    {
                        //有機率移除所有的負面元素效果
                        if (ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.Neutralization]))
                        {
                            RemoveBufferByType(ElementalBuff);
                            EffectEmitter.EmitParticle(GameManager.GM.PurifyParticle, Vector3.zero, Vector3.zero, transform);
                            addBuff = false;
                        }
                        //有機率施放元素過載
                        if (ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.Overload]))
                        {
                            OverloadSkil.LaunchAISpell();
                        }
                    }
                    if (addBuff)
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
        if (ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.Alchemy]))
        {
            BattleManage.ExtraDropGoldAdd(GameSettingData.GetEnemyDropGold(BattleManage.Floor));
        }
        //喝藥水隨機解除元素
        if (ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.Pharmacist]))
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
        if (ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.DrugFever]))
        {
            ExtraMoveSpeed += DrugFeverSpeedUp;
            EffectEmitter.EmitParticle(GameManager.GM.PotionSpeedUpParticle, Vector3.zero, Vector3.zero, transform);
        }
        switch (_data.Type)
        {
            case LootType.AvataEnergy:
                AvatarTimer += _data.Time * (1 + PotionEfficiency) + AvatarPotionBuff + MyEnchant[EnchantProperty.Allergy];
                break;
            case LootType.DamageUp:
                AddBuffer(RoleBuffer.DamageUp, _data.Time * (1 + PotionEfficiency) + MyEnchant[EnchantProperty.Allergy], _data.Value);
                break;
            case LootType.HPRecovery:
                if (HealthRatio == 1)
                {
                    if (MyEnchant[EnchantProperty.BloodyGold] > 0)
                        BattleManage.ExtraDropGoldAdd((int)(MyEnchant[EnchantProperty.BloodyGold] * (int)(MaxHealth * _data.Value * (1 + PotionEfficiency))));
                }
                HealHP((int)(MaxHealth * _data.Value * (1 + PotionEfficiency)));
                break;
            case LootType.Immortal:
                AddBuffer(RoleBuffer.Immortal, _data.Time * (1 + PotionEfficiency) + MyEnchant[EnchantProperty.Allergy]);
                break;
            case LootType.SpeedUp:
                AddBuffer(RoleBuffer.SpeedUp, _data.Time * (1 + PotionEfficiency) + MyEnchant[EnchantProperty.Allergy], _data.Value);
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
            if (ProbabilityGetter.GetResult(MyEnchant[EnchantProperty.Revive]))
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
    void ActiveEnchantSkill(bool _bool)
    {
        for (int i = 0; i < MyEnchantSkill.Count; i++)
        {
            MyEnchantSkill[i].enabled = _bool;
        }
    }
}
