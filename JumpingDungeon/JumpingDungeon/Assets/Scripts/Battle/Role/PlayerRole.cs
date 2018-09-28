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
    [Tooltip("護盾充飽需求時間(秒)")]
    [SerializeField]
    float ShieldReGenerateTime;
    float ShieldGenerateNum { get { return MaxShield / ShieldReGenerateTime; } }
    [Tooltip("護盾要沒受到攻擊多久時間(秒)才會開始充能")]
    [SerializeField]
    float ShieldRechargeTime;
    bool StartGenerateShield;
    MyTimer ShieldTimer;
    public override float MoveSpeed { get { return BaseMoveSpeed + ExtraMoveSpeed; } }
    private float extraMoveSpeed;
    public float ExtraMoveSpeed
    {
        get { return extraMoveSpeed; }
        set
        {
            if (value < 0)
                value = 0;
            else if (value > MaxEtraMove)
                value = MaxEtraMove;
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
    float MaxEtraMove;

    [Tooltip("變身時間")]
    [SerializeField]
    public float MaxAvaterTime;
    public float AvatarTimeRatio { get { return (float)AvatarTimer / (float)MaxAvaterTime; } }
    [SerializeField]
    Text AvatarTimerText;
    [Tooltip("變身時間加成(秒)")]
    [SerializeField]
    protected float AvatarTimeBuff;

    public virtual int EnergyDrop { get { return BaseEnergyDrop + ExtraEnergyDrop; } }
    public int ExtraEnergyDrop { get; protected set; }
    [Tooltip("能量掉落機率")]
    [SerializeField]
    protected int BaseEnergyDrop;
    public virtual int MoneyDrop { get { return BaseMoneyDrop + ExtraMoneyDrop; } }
    public int ExtraMoneyDrop { get; protected set; }
    [Tooltip("金幣掉落機率")]
    [SerializeField]
    protected int BaseMoneyDrop;
    public virtual float Bloodthirsty { get { return BaseBloodthirsty + ExtraBloodthirsty; } }
    public float ExtraBloodthirsty { get; protected set; }
    [Tooltip("吸血比例")]
    [SerializeField]
    protected float BaseBloodthirsty;
    [Tooltip("藥水效果強化比例")]
    [SerializeField]
    protected float PotionEfficacy;
    const int KeyboardMoveFactor = 1;
    const int CursorMoveFactor = 40;
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


    MyTimer AttackTimer;
    [HideInInspector]
    public int FaceDir;
    Dictionary<string, Skill> MonsterSkills = new Dictionary<string, Skill>();
    List<Skill> ActiveMonsterSkills = new List<Skill>();



    protected override void Awake()
    {
        base.Awake();
        AvatarTimer = MaxAvaterTime;
        AttackTimer = new MyTimer(DontAttackRestoreTime, RestoreAttack, null);
        ShieldTimer = new MyTimer(ShieldRechargeTime, ShieldRestore, null);
        ShieldBarWidth = ShieldBar.rect.width;
        Shield = MaxShield;
        InitMoveAfterimage();
        FaceDir = 1;
    }
    void InitMoveAfterimage()
    {
        MoveAfterimage = EffectEmitter.EmitParticle(MoveAfterimagePrefab, Vector3.zero, Vector3.zero, transform).GetComponentInChildrenExcludeSelf<ParticleSystem>();
        MoveAfterimage_Main = MoveAfterimage.main;
        MoveAfterimage_Main.maxParticles = 0;
        MoveAfterimage_Main.startLifetime = 0;
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
    public void AttackMotion()
    {
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
        CameraController.PlayMotion("Shake1");
        AudioPlayer.PlaySound(AttackSound);
    }
    public void BumpingAttack()
    {
        Vector2 force = MyRigi.velocity.normalized * SelfKnockForce * -1;
        MyRigi.AddForce(force);
        GetCondition(RoleBuffer.Stun, new BufferData(SelfSturnTime, 0));
    }
    protected override void Update()
    {
        base.Update();
        AvatarTimerFunc();
        AttackTimer.RunTimer();
        ShieldTimer.RunTimer();
        MonsterSkillTimerFunc();
        ShieldGenerate();
        ExtraMoveSpeedDecay();
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
                    FaceDir = 1;
                else
                    FaceDir = -1;
                float origAngle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) * Mathf.Deg2Rad;
                dir = new Vector3(Mathf.Cos(origAngle), Mathf.Sin(origAngle), 0).normalized;
                Vector2 force = dir * MoveSpeed * CursorMoveFactor;
                MyRigi.AddForce(force);
                //衝刺
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 rushForce = dir * RushForce;
                    MyRigi.AddForce(rushForce);
                    AudioPlayer.PlaySound(RushSound);
                }
            }
        }
        else if (ControlDevice == MoveControl.Keyboard)
        {
            //鍵盤移動
            float xMoveForce = 0;
            float yMoveForce = 0;
            xMoveForce = Input.GetAxis("Horizontal") * MoveSpeed * KeyboardMoveFactor;
            yMoveForce = Input.GetAxis("Vertical") * MoveSpeed * KeyboardMoveFactor;
            MyRigi.velocity += new Vector2(xMoveForce, yMoveForce);
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
                FaceDir = 1;
            else
                FaceDir = -1;
        }
        RoleTrans.localScale = new Vector2(FaceDir, 1);
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
                HealHP((int)(MaxHealth * _data.Value * (1 + PotionEfficacy)));
                break;
            case LootType.InvinciblePotion:
                GetCondition(RoleBuffer.Invicible, _data);
                break;
            case LootType.Money:
                break;
        }
    }
    public void InitMonsterSkill(string _name, Skill _skill)
    {
        if (!MonsterSkills.ContainsKey(_name))
        {
            Skill skill = gameObject.AddComponent(_skill.GetType()).CopySkill(_skill);
            skill.PlayerInitSkill();
            MonsterSkills.Add(_name, skill);
        }
    }
    public void GenerateMonsterSkill(string _name)
    {
        if (MonsterSkills.ContainsKey(_name))
        {
            MonsterSkills[_name].enabled = true;
            MonsterSkills[_name].PlayerGetSkill();
            ActiveMonsterSkills.Add(MonsterSkills[_name]);
        }
    }
    protected virtual void MonsterSkillTimerFunc()
    {
        for (int i = 0; i < ActiveMonsterSkills.Count; i++)
        {
            ActiveMonsterSkills[i].PSkillTimer -= Time.deltaTime;
            if (ActiveMonsterSkills[i].PSkillTimer <= 0)
            {
                if (MonsterSkills.ContainsKey(ActiveMonsterSkills[i].name))
                    MonsterSkills.Remove(ActiveMonsterSkills[i].PSkillName);
                ActiveMonsterSkills[i].InactivePlayerSkill();
                ActiveMonsterSkills.RemoveAt(i);
            }
        }
    }
    public void GetExtraMoveSpeed()
    {
        ExtraMoveSpeed += GainMoveFromKilling;
    }

    void ExtraMoveSpeedDecay()
    {
        if (ExtraMoveSpeed > 0)
        {
            float decay = (ExtraMoveSpeed / MoveDepletedTime);
            if (decay < 1)
                decay = 1;
            ExtraMoveSpeed -= Time.deltaTime * decay;
            MoveAfterimage_Main.maxParticles = Mathf.RoundToInt(ExtraMoveSpeed / 4);
            MoveAfterimage_Main.startLifetime = ExtraMoveSpeed / 200;
        }
    }

}
