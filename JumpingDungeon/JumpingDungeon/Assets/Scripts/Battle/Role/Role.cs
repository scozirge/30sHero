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
    protected const float MoveDecay = 0.5f;


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
    public virtual int Damage { get { return (int)(BaseDamage * (1 + DamageMultiple)); } }
    public float DamageMultiple
    {
        get
        {
            if (Buffers.ContainsKey(RoleBuffer.DamageBuff))
                return Buffers[RoleBuffer.DamageBuff].Value;
            else
                return 0;
        }
    }
    [Tooltip("基礎傷害")]
    [SerializeField]
    protected int BaseDamage;
    public virtual int Defence { get { return BaseDefence + ExtraDefence; } }
    public int ExtraDefence { get; protected set; }
    [Tooltip("基礎防禦")]
    [SerializeField]
    protected int BaseDefence;
    public virtual float MoveSpeed { get { return BaseMoveSpeed; } }
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



    public bool IsAlive { get; protected set; }
    public Dictionary<RoleBuffer, BufferData> Buffers = new Dictionary<RoleBuffer, BufferData>();


    protected virtual void Awake()
    {
        IsAlive = true;
        HPBarWidth = HealthBar.rect.width;
        Health = MaxHealth;
        MyForce = MyEnum.ParseEnum<Force>(gameObject.tag);
        MyRigi = GetComponent<Rigidbody2D>();
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
    }
    public virtual void BeAttack(int _dmg, Vector2 _force, Dictionary<RoleBuffer, BufferData> buffers)
    {
        //Add KnockForce
        MyRigi.velocity = Vector2.zero;
        MyRigi.velocity = _force;
        //if Invincible, can't take damage and debuff
        if (Buffers.ContainsKey(RoleBuffer.Invicible))
            return;
        //take damage
        ReceiveDmg(_dmg);
        //Get conditions
        if (buffers != null)
        {
            List<RoleBuffer> keyList = new List<RoleBuffer>(buffers.Keys);
            for (int i = 0; i < keyList.Count; i++)
            {
                GetCondition(keyList[i], buffers[keyList[i]]);
            }
        }
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
    public virtual void GetCondition(RoleBuffer _condition, BufferData _data)
    {
        if (Buffers.ContainsKey(_condition))
        {
            if (Buffers[_condition].Time < _data.Time)
                Buffers[_condition].Time = _data.Time;
        }
        else
        {
            Buffers.Add(_condition, _data);
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
                Buffers.Remove(keyList[i]);
                keyList.RemoveAt(i);
            }
        }
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
