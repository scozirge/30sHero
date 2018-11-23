using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("攻擊特效")]
    [SerializeField]
    ParticleSystem AttackEffect;
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

    public void SetRange()
    {
        RangeCol.radius = RangeCol.radius * (1 + Attacker.AttackRangeProportion);
    }
    void OnTriggerEnter2D(Collider2D _col)
    {
        if (Attacker.BuffersExist(RoleBuffer.Untouch))
            return;
        if (_col.gameObject.tag == Force.Enemy.ToString())
        {
            if (Attacker.IsAvatar)
            {
                EnemyRole er = _col.GetComponent<EnemyRole>();
                float extraDamageProportion = 0;
                //生命低時機率強化傷害
                if (Attacker.HealthRatio < 0.4f && ProbabilityGetter.GetResult(Attacker.BerserkerProportion))
                {
                    extraDamageProportion += 0.5f;
                    Attacker.SetBerserkerBladeLight(true);
                }
                else
                    Attacker.SetBerserkerBladeLight(false);
                //衝刺加傷害
                if (Attacker.OnRush)
                    extraDamageProportion += Attacker.LethalDashProportion;
                //火焰刀增加傷害&特效
                if (Attacker.BuffersExist(RoleBuffer.Burn))
                {
                    extraDamageProportion += Attacker.FireBladeProportion;
                    EffectEmitter.EmitParticle(GameManager.GM.FireBladeParticle, er.transform.position, Vector3.zero, null);
                }
                //菁英獵殺
                if(Attacker.EliteHuntingProportion>0)
                {
                    if (Attacker.LastTarget.Hit(er))
                    {
                        extraDamageProportion += Attacker.EliteHuntingProportion;
                        EffectEmitter.EmitParticle(GameManager.GM.ClawParticle, Vector3.zero, Vector3.zero, er.transform);
                    }
                }
                BeforeAttackAction(er);
                Vector2 force = (er.transform.position - transform.position).normalized * KnockForce;
                int causeDamage = (int)(Attacker.Damage * DamagePercent * (1 + extraDamageProportion));
                er.BeAttack(Attacker.MyForce, ref causeDamage, force);
                Attacker.HealFromCauseDamage(causeDamage);
                if (er.IsAlive)
                {
                    Attacker.BumpingAttack();
                    AfterAttackAction_TargetAlive(er);
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
        if (ProbabilityGetter.GetResult(Attacker.BurningWeaponProportion))
            _er.AddBuffer(RoleBuffer.Burn, 5);
        if (ProbabilityGetter.GetResult(Attacker.PoisonedWeaponProportion))
            _er.AddBuffer(RoleBuffer.DamageDown, 5);
        if (ProbabilityGetter.GetResult(Attacker.FrozenWeaponProportion))
            _er.AddBuffer(RoleBuffer.Freeze, 5);
        if (ProbabilityGetter.GetResult(Attacker.StunningSlashProportion))
            _er.AddBuffer(RoleBuffer.Stun, 1.5f);

    }
    void TargetDieAction(EnemyRole _er)
    {
        if (ProbabilityGetter.GetResult(Player.GetEnchantProperty(EnchantProperty.ExtralGoldDrop)))
        {
            _er.ExtralGoldDrop();
        }
        if (Attacker.CarnivorousProportion > 0)
            Attacker.ExtendMaxHP((int)(Attacker.CarnivorousProportion * Attacker.MaxHealth));
    }
}
