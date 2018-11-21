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
                BeforeAttackAction(er);
                Vector2 force = (er.transform.position - transform.position).normalized * KnockForce;
                int causeDamage = (int)(Attacker.Damage * DamagePercent);
                er.BeAttack(Attacker.MyForce, ref causeDamage, force);
                Attacker.HealFromCauseDamage(causeDamage);
                if (er.IsAlive)
                {
                    Attacker.BumpingAttack();
                    AfterAttackAction(er);
                }
                else
                {
                    if (ProbabilityGetter.GetResult(Player.GetEnchantProperty(EnchantProperty.ExtralGoldDrop)))
                    {
                        er.ExtralGoldDrop();
                    }
                }
                int faceDir = 1;
                if ((Attacker.transform.position.x - er.transform.position.x) > 0)
                    faceDir = -1;
                Attacker.Face(faceDir);
                Attacker.AttackMotion();
                //SpawnAttackEffect
                if (!AttackEffect)
                    return;
                Vector2 pos = Vector2.Lerp(er.transform.position, Attacker.transform.position, 0.5f);
                EffectEmitter.EmitParticle(AttackEffect, pos, Vector3.zero, null);
            }
            else
                Attacker.BumpingAttack();
        }
    }
    void BeforeAttackAction(EnemyRole _er)
    {
    }
    void AfterAttackAction(EnemyRole _er)
    {
        if (ProbabilityGetter.GetResult(Attacker.BurningWeaponProportion))
            _er.AddBuffer(RoleBuffer.Burn, 5);
        if (ProbabilityGetter.GetResult(Attacker.PoisonedWeaponProportion))
            _er.AddBuffer(RoleBuffer.DamageDown, 5);
        if (ProbabilityGetter.GetResult(Attacker.FrozenWeaponProportion))
            _er.AddBuffer(RoleBuffer.Freeze, 5);
        if (ProbabilityGetter.GetResult(1))
            _er.AddBuffer(RoleBuffer.Stun, 1.5f);
    }
}
