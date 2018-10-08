using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimationPlayer))]
public abstract class Role : MonoBehaviour
{
    [Tooltip("勢力")]
    public Force MyForce;
    [SerializeField]
    protected RectTransform HealthBar;
    float HPBarWidth;
    protected Rigidbody2D MyRigi;
    [SerializeField]
    protected float MoveDecay = 0.5f;

    [HideInInspector]
    public Direction DirectX;
    [HideInInspector]
    public Direction DirectY;
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            if (value < 0)
                value = 0;
            else if (value > MaxHealth)
                value = MaxHealth;
            health = value;
            HealthBar.sizeDelta = new Vector2(HPBarWidth * HealthRatio, HealthBar.rect.height);
        }
    }
    [Tooltip("基礎血量")]
    [SerializeField]
    protected int BaseHealth;
    public int ExtraHealth { get; protected set; }
    public int MaxHealth { get { return BaseHealth + ExtraHealth; } }
    public float HealthRatio { get { return (float)Health / (float)MaxHealth; } }
    public virtual int Damage { get {
        return (int)(BaseDamage * 
        (1 + (Buffers.ContainsKey(RoleBuffer.DamageBuff)?Buffers[RoleBuffer.DamageBuff].Value:0+
        (Buffers.ContainsKey(RoleBuffer.Curse)?-GameSettingData.CurseDamageReduce:0)))); } }
    [Tooltip("基礎傷害")]
    [SerializeField]
    protected int BaseDamage;
    public virtual int Defence { get { return BaseDefence + ExtraDefence; } }

    [Tooltip("基礎防禦")]
    [SerializeField]
    protected int BaseDefence;
    public virtual float MoveSpeed { get { return BaseMoveSpeed * (1 + (Buffers.ContainsKey(RoleBuffer.Freeze)?
        -GameSettingData.FreezeMove:0  )); } }
    [Tooltip("基礎移動速度")]
    [SerializeField]
    protected int BaseMoveSpeed;
    [Tooltip("死亡特效")]
    [SerializeField]
    ParticleSystem DeathEffect;
    [SerializeField]
    protected AnimationPlayer AniPlayer;
    [SerializeField]
    protected Transform RoleTrans;
    [Tooltip("死亡音效")]
    [SerializeField]
    AudioClip DeathSound;

    MyTimer BurningTimer;
    public float DamageBuff { get; protected set; }

    public int ExtraDefence { get; protected set; }

    public bool IsAlive { get; protected set; }
    public Dictionary<RoleBuffer, BufferData> Buffers = new Dictionary<RoleBuffer, BufferData>();
    Dictionary<RoleBuffer, ParticleSystem> BufferParticles = new Dictionary<RoleBuffer, ParticleSystem>();
    protected List<Skill> Skills = new List<Skill>();

    protected virtual void Start()
    {
        if (MoveDecay <= 0.2f)
            MoveDecay = 0.2f;
        IsAlive = true;
        HPBarWidth = HealthBar.rect.width;
        Health = MaxHealth;
        MyForce = MyEnum.ParseEnum<Force>(gameObject.tag);
        MyRigi = GetComponent<Rigidbody2D>();
        Skill[] coms = GetComponents<Skill>();
        Skills = coms.ToList<Skill>();
        BurningTimer = new MyTimer(GameSettingData.BurnInterval, Burn, false, false);
        BurningTimer.StartRunTimer = false;
    }
    void Burn()
    {
        BurningTimer.StartRunTimer = true;
        ReceiveDmg((int)(Health * GameSettingData.BurnDamage));
    }
    protected virtual void Move()
    {
        MyRigi.velocity *= MoveDecay;
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }
    protected virtual void Update()
    {
        ConditionTimerFunc();
        BurningTimer.RunTimer();
    }

    public virtual void BeAttack(int _dmg, Vector2 _force)
    {
        //Add KnockForce
        MyRigi.velocity = Vector2.zero;
        MyRigi.velocity = _force;
        if (EvitableAttack())
            return;
        //ShieldBlock
        ShieldBlock(ref _dmg);
        //take damage
        ReceiveDmg(_dmg);
    }
    protected virtual bool EvitableAttack()
    {
        //if Immortal or Block, can't take damage
        if (Buffers.ContainsKey(RoleBuffer.Immortal))
            return true;
        if (Buffers.ContainsKey(RoleBuffer.Block))
        {
            if (--Buffers[RoleBuffer.Block].Value <= 0)
            {
                RemoveBuffer(Buffers[RoleBuffer.Block]);
            }
            return true;
        }
        return false;
    }
    protected virtual void ShieldBlock(ref int _dmg)
    {
    }
    public virtual void ReceiveDmg(int _dmg)
    {
        if (!IsAlive)
            return;
        Health -= _dmg;
        DeathCheck();
    }
    public virtual void HealHP(int _heal)
    {
        if (!IsAlive)
            return;
        Health += _heal;
    }
    protected virtual bool DeathCheck()
    {
        if (Health <= 0)
        {
            IsAlive = false;
            SelfDestroy();
        }
        else IsAlive = true;
        return !IsAlive;
    }
    public void AddBuffer(RoleBuffer _bufferType, params float[] _values)
    {
        if (_values.Length > 0 && _values[0] != 0)
        {
            switch (_values.Length)
            {
                case 1:
                    AddBuffer(new BufferData(_bufferType, _values[0]));
                    break;
                case 2:
                    AddBuffer(new BufferData(_bufferType, _values[0], _values[1]));
                    break;
            }
        }
    }
    public virtual void AddBuffer(BufferData _buffer)
    {
        if (Buffers.ContainsKey(_buffer.Type))
        {
            Buffers[_buffer.Type] = _buffer;
        }
        else
        {
            Buffers.Add(_buffer.Type, _buffer);
            if (!BufferParticles.ContainsKey(_buffer.Type))
            {
                ParticleSystem ps = EffectEmitter.EmitParticle(GameManager.GetBufferParticle(_buffer.Type), Vector3.zero, Vector3.zero, transform);
                BufferParticles.Add(_buffer.Type, ps);
            }
            BufferEffectChange(_buffer, true);
        }
    }
    protected virtual void ConditionTimerFunc()
    {
        List<RoleBuffer> keyList = new List<RoleBuffer>(Buffers.Keys);
        for (int i = 0; i < keyList.Count; i++)
        {
            Buffers[keyList[i]].Time -= Time.deltaTime;
            if (Buffers[keyList[i]].Time <= 0)
            {
                RemoveBuffer(Buffers[keyList[i]]);
            }
        }
    }
    public virtual void RemoveBuffer(BufferData _buffer)
    {
        Buffers.Remove(_buffer.Type);
        Destroy(BufferParticles[_buffer.Type].gameObject);
        BufferParticles.Remove(_buffer.Type);
        BufferEffectChange(_buffer, false);
    }
    void BufferEffectChange(BufferData _buffer, bool _add)
    {
        switch (_buffer.Type)
        {
            case RoleBuffer.Stun:
                for (int i = 0; i < Skills.Count; i++)
                {
                    Skills[i].SetCanAttack(!_add);
                }
                break;
            case RoleBuffer.Burn:
                if (_add)
                    Burn();
                else
                {
                    BurningTimer.RestartCountDown();
                    BurningTimer.StartRunTimer = false;
                }
                break;
        }

    }
    public void RemoveAllBuffer()
    {
        List<RoleBuffer> keyList = new List<RoleBuffer>(Buffers.Keys);
        for (int i = 0; i < keyList.Count; i++)
        {
            BufferEffectChange(Buffers[keyList[i]], false);
        }
        Buffers = new Dictionary<RoleBuffer, BufferData>();
    }
    protected virtual void SelfDestroy()
    {
        EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
        AudioPlayer.PlaySound(DeathSound);
        Destroy(gameObject);
    }
    public virtual void Attack()
    {
    }
    public virtual void PreAttack()
    {
    }
}
