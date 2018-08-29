using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public abstract class Role : MonoBehaviour
{
    [SerializeField]
    Force MyForce;
    [SerializeField]
    protected int Move;
    [SerializeField]
    RectTransform HealthBar;

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
    public bool IsAlive { get; protected set; }

    protected virtual void Awake()
    {
        IsAlive = true;
        MaxHealth = 100;
        BaseDamage = 20;
        Health = MaxHealth;
    }

    public virtual void Update()
    {

    }
    protected virtual void Attack()
    {

    }
    public virtual void BeAttack(int _dmg)
    {
        //EffectEmitter.EmitParticle("hitEffect", transform.position, Vector3.zero, null);
        ReceiveDmg(_dmg);
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
}
