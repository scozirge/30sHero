using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public abstract class Role : MonoBehaviour
{
    [SerializeField]
    public Force MyForce;
    [SerializeField]
    protected RectTransform HealthBar;
    [SerializeField]
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
            health = value;
        }
    }
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
                return (int)Buffers[RoleBuffer.DamageBuff].Value;
            else
                return 0;
        }
    }
    [SerializeField]
    protected int BaseDamage;
    public virtual int Defence { get { return BaseDefence + ExtraDefence; } }
    public int ExtraDefence { get; protected set; }
    [SerializeField]
    protected int BaseDefence;
    public virtual int MoveSpeed { get { return BaseMoveSpeed + ExtraMoveSpeed; } }
    public int ExtraMoveSpeed { get; protected set; }
    [SerializeField]
    protected int BaseMoveSpeed;

    [SerializeField]
    ParticleSystem DeathEffect;
    [SerializeField]
    protected AnimationPlayer AniPlayer;

    public bool IsAlive { get; protected set; }
    public Dictionary<RoleBuffer, BufferData> Buffers = new Dictionary<RoleBuffer, BufferData>();


    protected virtual void Awake()
    {
        IsAlive = true;
        Health = MaxHealth;
    }
    protected virtual void Move()
    {
        MyRigi.velocity *= MoveDecay;
    }
    protected virtual void Update()
    {
        Move();
        ConditionTimerFunc();
    }
    public virtual void BeAttack(int _dmg, Vector2 _force, Dictionary<RoleBuffer, BufferData> buffers)
    {
        if (Buffers.ContainsKey(RoleBuffer.Invicible))
        {
            return;
        }
        ReceiveDmg(_dmg);
        MyRigi.velocity = Vector2.zero;

        //Get conditions
        if (buffers != null)
        {
            List<RoleBuffer> keyList = new List<RoleBuffer>(buffers.Keys);
            for (int i = 0; i < keyList.Count; i++)
            {
                GetCondition(keyList[i], buffers[keyList[i]]);
            }
        }
        //Add KnockForce
        MyRigi.velocity = _force;
    }
    public virtual void ReceiveDmg(int _dmg)
    {
        if (!IsAlive)
            return;
        Health -= _dmg;
        HealthBar.localScale = new Vector2(HealthRatio, 1);
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
        Destroy(gameObject);
    }
    public virtual void Attack()
    {

    }
}
