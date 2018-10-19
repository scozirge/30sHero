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


    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == Force.Enemy.ToString())
        {
            if(Attacker.IsAvatar)
            {
                Role er = _col.GetComponent<Role>();
                Vector2 force = (er.transform.position - transform.position).normalized * KnockForce;
                er.BeAttack(Attacker.Damage, force);
                if (er.IsAlive)
                    Attacker.BumpingAttack();
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
}
