using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    ParticleSystem AttackEffect;
    [SerializeField]
    PlayerRole Attacker;
    [SerializeField]
    float KnockForce;


    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == Force.Enemy.ToString())
        {
            Role er = _col.GetComponent<Role>();
            Vector2 force = (er.transform.position - transform.position).normalized * KnockForce;
            er.BeAttack(Attacker.Damage, force, null);
            Attacker.Attack();
            //SpawnAttackEffect
            if (!AttackEffect)
                return;
            Vector2 pos = Vector2.Lerp(er.transform.position, Attacker.transform.position, 0.5f);
            EffectEmitter.EmitParticle(AttackEffect, pos, Vector3.zero, null);
        }
    }
}
