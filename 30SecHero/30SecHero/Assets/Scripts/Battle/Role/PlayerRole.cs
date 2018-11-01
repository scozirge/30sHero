using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class PlayerRole : Role
{
    [Tooltip("使用測試模式，非測試模式時，玩家數值為讀表")]
    [SerializeField]
    bool TestMode;
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
    public override float MoveSpeed { get { return base.MoveSpeed + ExtraMoveSpeed; } }
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
    [Tooltip("金幣掉落機率")]
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

    ParticleSystem CurBeHitEffect;
    MyTimer AttackTimer;
    MyTimer JumpTimer;
    [HideInInspector]
    public int FaceLeftOrRight;
    Dictionary<string, Skill> MonsterSkills = new Dictionary<string, Skill>();
    Dictionary<string, Soul> MonsterSouls = new Dictionary<string, Soul>();
    public EnemyRole ClosestEnemy;
    bool CanJump;

    protected override void Start()
    {
        InitPlayerProperties();
        base.Start();
        AvatarTimer = MaxAvaterTime;
        AttackTimer = new MyTimer(DontAttackRestoreTime, RestoreAttack, false, false);
        ShieldTimer = new MyTimer(ShieldRechargeTime, ShieldRestore, false, false);
        JumpTimer = new MyTimer(JumpCDTime, SetCanJump, false, false);

        ShieldBarWidth = ShieldBar.rect.width;
        Health = MaxHealth;
        Shield = MaxShield;
        InitMoveAfterimage();
        FaceLeftOrRight = 1;
        IsAvatar = true;
        CanJump = true;
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
    void ShieldGenerate()
    {
        if (Shield < MaxShield)
            if (StartGenerateShield)
                Shield += ShieldGenerateProportion * MaxShield * Time.deltaTime;
    }
    protected void ChangeToStopDrag()
    {
        if (!DragTimer.StartRunTimer)
        {
            MyRigi.drag = StopDrag;
        }
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
            IsAlive = false;
            SelfDestroy();
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
        base.BeAttack(_attackerForce, ref _dmg, _force);
    }
    public override void ReceiveDmg(ref int _dmg)
    {
        base.ReceiveDmg(ref _dmg);
    }
    protected override void ShieldBlock(ref int _dmg)
    {
        base.ShieldBlock(ref _dmg);
        if (Shield != 0)
        {
            if (CurBeHitEffect) Destroy(CurBeHitEffect.gameObject);
            if (BeHitEffect_Shield) CurBeHitEffect = EffectEmitter.EmitParticle(BeHitEffect_Shield, Vector2.zero, Vector3.zero, transform);
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
            IsAvatar = false;
            RemoveAllSill();
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
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
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
        RoleTrans.localScale = new Vector2(FaceLeftOrRight, 1);
    }
    public void GetLoot(LootData _data)
    {
        switch (_data.Type)
        {
            case LootType.AvataEnergy:
                AvatarTimer += _data.Time * (1 + PotionEfficiency) + AvatarPotionBuff;
                break;
            case LootType.DamageUp:
                AddBuffer(RoleBuffer.DamageUp, _data.Time * (1 + PotionEfficiency), _data.Value);
                break;
            case LootType.HPRecovery:
                HealHP((int)(MaxHealth * _data.Value * (1 + PotionEfficiency)));
                break;
            case LootType.Immortal:
                AddBuffer(RoleBuffer.Immortal, _data.Time * (1 + PotionEfficiency));
                break;
            case LootType.SpeedUp:
                AddBuffer(RoleBuffer.SpeedUp, _data.Time * (1 + PotionEfficiency), _data.Value);
                break;
        }
        AttackMotion();
    }
    public void GetResource(ResourceType _type, int _value)
    {
        switch (_type)
        {
            case ResourceType.Gold:
                BattleManage.EnemyDropGoldAdd((int)(_value * (1 + GoldDrop)));
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
            ActiveMonsterSkills.Add(MonsterSkills[_name]);
            if (!MonsterSouls.ContainsKey(_name))
            {
                Soul soul = SoulSpawner.SpawnSoul(transform.position);
                soul.Init(transform, _spritePath);
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

}
