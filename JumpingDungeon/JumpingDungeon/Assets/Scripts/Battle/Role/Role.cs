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
    public int MaxHealth { get; protected set; }
    public float HealthRatio { get { return (float)Health / (float)MaxHealth; } }
    public virtual int Damage { get { return BaseDamage + ExtraDamage; } }
    public int ExtraDamage { get; protected set; }
    public int BaseDamage { get; protected set; }
    public int MoveSpeed { get; protected set; }
    public bool IsAlive { get; protected set; }
    public Dictionary<RoleCondition, float> Conditions = new Dictionary<RoleCondition, float>();

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
    protected virtual void Attack()
    {

    }
    public virtual void BeAttack(int _dmg,Vector2 _force)
    {
        //EffectEmitter.EmitParticle("hitEffect", transform.position, Vector3.zero, null);
        ReceiveDmg(_dmg);
        MyRigi.velocity = Vector2.zero;
        GetCondition(RoleCondition.Stun, 0.5f);
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
    public virtual void GetCondition(RoleCondition _condition,float _duration)
    {
        if (Conditions.ContainsKey(_condition))
            return;
        Conditions.Add(_condition, _duration);
    }
    protected virtual void ConditionTimerFunc()
    {
        List<RoleCondition> keyList=new List<RoleCondition>(Conditions.Keys);
        for(int i=0;i<keyList.Count;i++)
        {
            Conditions[keyList[i]] -= Time.deltaTime;
            if (Conditions[keyList[i]] <= 0)
            {
                Conditions.Remove(keyList[i]);
                keyList.RemoveAt(i);
            }
        }
    }
}
