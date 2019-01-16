using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("攻擊特效")]
    [SerializeField]
    ParticleSystem AttackEffect;
    [Tooltip("衝刺攻擊特效")]
    [SerializeField]
    ParticleSystem RushAttackEffect;
    [SerializeField]
    PlayerRole Attacker;
    [Tooltip("擊退力道")]
    [SerializeField]
    float KnockForce;
    [Tooltip("傷害倍率(傷害=傷害倍率x腳色攻擊力))")]
    [SerializeField]
    protected float DamagePercent = 1;
    [SerializeField]
    CircleCollider2D RangeCol;
    [Tooltip("傷害間隔，最低為0.1")]
    [SerializeField]
    protected float DamageInterval = 0.3f;
    protected List<MyTimer> ReadyToDamageTimers;
    protected Dictionary<string, bool> ReadyToDamageTargets;

    void Start()
    {
        ReadyToDamageTargets = new Dictionary<string, bool>();
        ReadyToDamageTimers = new List<MyTimer>();
        if (DamageInterval < 0.1f)
            DamageInterval = 0.1f;
    }
    void Update()
    {
        for (int i = 0; i < ReadyToDamageTimers.Count; i++)
        {
            ReadyToDamageTimers[i].RunTimer();
        }
    }
    public void SetRange()
    {
        RangeCol.radius = RangeCol.radius * (1 + Attacker.MyEnchant[EnchantProperty.AttackRange]);
    }
    void OnTriggerStay2D(Collider2D _col)
    {
        if (Attacker.BuffersExist(RoleBuffer.Untouch))
            return;
        if (_col.gameObject.tag == Force.Enemy.ToString())
        {
            if (Attacker.IsAvatar)
            {
                if (!CheckReadyToDamageTarget(_col))
                    return;
                EnemyRole er = _col.GetComponent<EnemyRole>();
                float extraDamageProportion = 0;
                //衝擊斬(攻擊有機率施放)
                if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.ChopStrike]))
                {
                    Attacker.ChopStrikeSkill.SetLockDirection(er);
                    Attacker.ChopStrikeSkill.LaunchAISpell();
                }
                //生命低時機率強化傷害
                if (Attacker.HealthRatio < 0.4f && ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.Berserker]))
                {
                    extraDamageProportion += 0.3f;
                    Attacker.SetBerserkerBladeLight(true);
                }
                else
                    Attacker.SetBerserkerBladeLight(false);
                //衝刺攻擊附帶效果
                if (Attacker.OnRush)
                {
                    EffectEmitter.EmitParticle(RushAttackEffect, er.transform.position, Vector3.zero, null);
                    //衝刺加傷害
                    extraDamageProportion += Attacker.MyEnchant[EnchantProperty.LethalDash];

                }

                //火焰刀增加傷害&特效
                if (Attacker.BuffersExist(RoleBuffer.Burn))
                {
                    extraDamageProportion += Attacker.MyEnchant[EnchantProperty.FireBlade];
                    EffectEmitter.EmitParticle(GameManager.GM.FireBladeParticle, er.transform.position, Vector3.zero, null);
                }
                if (Attacker.LastTarget.Hit(er))
                {
                    //菁英獵殺
                    if (Attacker.MyEnchant[EnchantProperty.EliteHunting] > 0)
                    {
                        extraDamageProportion += Attacker.MyEnchant[EnchantProperty.EliteHunting];
                        EffectEmitter.EmitParticle(GameManager.GM.ClawParticle, Vector3.zero, Vector3.zero, er.transform);
                    }
                }
                BeforeAttackAction(er);
                Vector2 force = (er.transform.position - transform.position).normalized * KnockForce;
                Debug.Log("Attacker.WeaponAttack=" + Attacker.WeaponAttack);
                int causeDamage = (int)((Attacker.Damage + Attacker.WeaponAttack) * DamagePercent * (1 + extraDamageProportion));
                er.BeAttack(Attacker.MyForce, ref causeDamage, force);
                Attacker.HealFromCauseDamage(causeDamage);
                if (er.IsAlive)
                {
                    Attacker.BumpingAttack();
                    AfterAttackAction_TargetAlive(er);
                    //衝刺攻擊會暈眩目標
                    if (Attacker.OnRush)
                    {
                        er.AddBuffer(RoleBuffer.Stun, GameSettingData.RushStun);                        
                    }
                }
                else
                {
                    TargetDieAction(er);
                }
                int faceDir = 1;
                if ((Attacker.transform.position.x - er.transform.position.x) > 0)
                    faceDir = -1;
                Attacker.Face(faceDir);
                Attacker.AttackMotion();
                //刀子特效
                if (AttackEffect)
                {
                    Vector2 pos = Vector2.Lerp(er.transform.position, Attacker.transform.position, 0.5f);
                    EffectEmitter.EmitParticle(AttackEffect, pos, Vector3.zero, null);
                }
            }
            else
                Attacker.BumpingAttack();
        }
    }
    void BeforeAttackAction(EnemyRole _er)
    {
    }
    void AfterAttackAction_TargetAlive(EnemyRole _er)
    {
        if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.BurningWeapon]))
            _er.AddBuffer(RoleBuffer.Burn, 5);
        if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.PoisonedWeapon]))
            _er.AddBuffer(RoleBuffer.DamageDown, 5);
        if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.FrozenWeapon]))
            _er.AddBuffer(RoleBuffer.Freeze, 5);
        if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.StunningSlash]))
            _er.AddBuffer(RoleBuffer.Stun, 1.5f);
    }
    void TargetDieAction(EnemyRole _er)
    {
        //點石成金(刀子擊殺有機率掉落金幣)
        if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.ExtralGoldDrop]))
        {
            _er.ExtralGoldDrop();
        }
        //肉食(刀子殺怪增加最大血量)
        if (Attacker.MyEnchant[EnchantProperty.Carnivorous] > 0)
            Attacker.ExtendMaxHP((int)(Attacker.MyEnchant[EnchantProperty.Carnivorous] * Attacker.OriginMaxHealth));
        //元素擊殺產生爆炸
        if (_er.BuffersExist(RoleBuffer.Burn))
        {
            if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.FireChop]))
            {
                Attacker.FireChopSkil.LaunchAISpell();
            }
        }
        else if (_er.BuffersExist(RoleBuffer.Freeze))
        {
            if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.FrozenChop]))
            {
                Attacker.FrozenChopSkil.LaunchAISpell();
            }
        }
        else if (_er.BuffersExist(RoleBuffer.DamageDown))
        {
            if (ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.PoisonChop]))
            {
                Attacker.PoisonChopSkil.LaunchAISpell();
            }
        }
        //機率產生衝擊波
        if (Attacker.OnRush && ProbabilityGetter.GetResult(Attacker.MyEnchant[EnchantProperty.DashImpact]))
        {
            Attacker.DashImpactSkil.LaunchAISpell();
        }

    }

    protected bool CheckReadyToDamageTarget(Collider2D _target)
    {
        string id = _target.GetInstanceID().ToString();
        if (ReadyToDamageTargets.ContainsKey(id))
        {
            if (ReadyToDamageTargets[id])
            {
                ReadyToDamageTargets[id] = false;
                return true;
            }
            else
                return false;
        }
        else
        {
            if (DamageInterval > 0)
                ReadyToDamageTimers.Add(new MyTimer(DamageInterval, ReadyToDamageTimeOutFunc, true, true, id));
            ReadyToDamageTargets.Add(id, false);
            return true;
        }
    }
    protected void ReadyToDamageTimeOutFunc(string _key)
    {
        if (ReadyToDamageTargets.ContainsKey(_key))
        {
            ReadyToDamageTargets[_key] = true;
        }
    }
}
