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
    [SerializeField]
    protected Image[] AttackImages;
    [SerializeField]
    protected Image[] StayImages;
    [SerializeField]
    protected Image[] MoveImages;

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
    public int MaxHealth { get; protected set; }
    public float HealthRatio { get { return (float)Health / (float)MaxHealth; } }
    public virtual int Damage { get { return BaseDamage + ExtraDamage; } }
    public int ExtraDamage { get; protected set; }
    public int BaseDamage { get; protected set; }
    public virtual int Defence { get { return BaseDefence + ExtraDefence; } }
    public int ExtraDefence { get; protected set; }
    public int BaseDefence { get; protected set; }
    public virtual int MoveSpeed { get { return BaseMoveSpeed + ExtraMoveSpeed; } }
    public int ExtraMoveSpeed { get; protected set; }
    public int BaseMoveSpeed { get; protected set; }
    public virtual int AvatarTime { get { return BaseAvatarTime + ExtraAvatarTime; } }
    public int ExtraAvatarTime { get; protected set; }
    public int BaseAvatarTime { get; protected set; }
    public virtual int EnergyDrop { get { return BaseEnergyDrop + ExtraEnergyDrop; } }
    public int ExtraEnergyDrop { get; protected set; }
    public int BaseEnergyDrop { get; protected set; }
    public virtual int MoneyDrop { get { return BaseMoneyDrop + ExtraMoneyDrop; } }
    public int ExtraMoneyDrop { get; protected set; }
    public int BaseMoneyDrop { get; protected set; }
    public virtual int Bloodthirsty { get { return BaseBloodthirsty + ExtraBloodthirsty; } }
    public int ExtraBloodthirsty { get; protected set; }
    public int BaseBloodthirsty { get; protected set; }
    public virtual int PotionEfficacy { get { return BasePotionEfficacy + ExtraPotionEfficacy; } }
    public int ExtraPotionEfficacy { get; protected set; }
    public int BasePotionEfficacy { get; protected set; }


    public bool IsAlive { get; protected set; }
    public Dictionary<RoleBuffer, BufferData> Buffers = new Dictionary<RoleBuffer, BufferData>();

    protected virtual void Awake()
    {
        IsAlive = true;
        MaxHealth = 100;
        BaseDamage = 20;
        Health = MaxHealth;
    }

    protected virtual void Update()
    {
        MyRigi.velocity *= MoveDecay;
        ConditionTimerFunc();
    }
    public virtual void AttackReaction()
    {
        Vector2 force = MyRigi.velocity.normalized * 100000 * -1;
        MyRigi.AddForce(force);
        GetCondition(RoleBuffer.Stun, new BufferData(0.3f, 0));
    }
    public virtual void BeAttack(int _dmg, Vector2 _force,Dictionary<RoleBuffer,BufferData> buffers)
    {
        //EffectEmitter.EmitParticle("hitEffect", transform.position, Vector3.zero, null);
        ReceiveDmg(_dmg);
        MyRigi.velocity = Vector2.zero;

        //Get conditions
        if(buffers!=null)
        {
            List<RoleBuffer> keyList = new List<RoleBuffer>(buffers.Keys);
            for (int i = 0; i < keyList.Count; i++)
            {
                GetCondition(keyList[i], buffers[keyList[i]]);
            }
        }
        //Add KnockForce
        MyRigi.AddForce(_force);
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
            //EffectEmitter.EmitParticle("deathEffect", transform.position, Vector3.zero, null);
        }
        else IsAlive = true;
        return !IsAlive;
    }
    public virtual void GetCondition(RoleBuffer _condition, BufferData _data)
    {
        if (Buffers.ContainsKey(_condition))
        {
            if (Buffers[_condition].Duration < _data.Duration)
                Buffers[_condition].Duration = _data.Duration;            
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
            Buffers[keyList[i]].Duration -= Time.deltaTime;
            if (Buffers[keyList[i]].Duration <= 0)
            {
                Buffers.Remove(keyList[i]);
                keyList.RemoveAt(i);
            }
        }
    }
}
