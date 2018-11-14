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


    void OnTriggerEnter2D(Collider2D _col)
    {
        if (Attacker.BuffersExist(RoleBuffer.Untouch))
            return;
        if (_col.gameObject.tag == Force.Enemy.ToString())
        {
            if (Attacker.IsAvatar)
            {
                EnemyRole er = _col.GetComponent<EnemyRole>();
                Vector2 force = (er.transform.position - transform.position).normalized * KnockForce;
                int causeDamage = (int)(Attacker.Damage * DamagePercent);
                er.BeAttack(Attacker.MyForce, ref causeDamage, force);
                Attacker.HealFromCauseDamage(causeDamage);
                if (er.IsAlive)
                {
                    Attacker.BumpingAttack();
                    if (ProbabilityGetter.GetResult(Attacker.ElementalBladeProportion))
                    {
                        er.AddBuffer(GetRandomElementDebuff());
                    }
                }
                else
                {
                    if (ProbabilityGetter.GetResult(Player.GetEnchantProperty(EnchantProperty.ExtralGoldDrop)))
                    {
                        er.ExtralGoldDrop();
                    }
                }
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
    BufferData GetRandomElementDebuff()
    {
        int rand = Random.Range(0, 3);
        BufferData bd = new BufferData(RoleBuffer.Burn, 5);
        switch(rand)
        {
            case 0:
                bd = new BufferData(RoleBuffer.Burn, 5);
                break;
            case 1:
                bd = new BufferData(RoleBuffer.Freeze, 5);
                break;
            case 2:
                bd = new BufferData(RoleBuffer.DamageDown, 5);
                break;
        }
        return bd;
    }
}
