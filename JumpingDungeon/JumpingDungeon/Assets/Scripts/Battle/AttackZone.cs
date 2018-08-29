using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour {

    [SerializeField]
    Role Attacker;
    [SerializeField]
    Force TargetType;

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag==TargetType.ToString())
        {
            Role er = _col.GetComponent<Role>();
            er.BeAttack(Attacker.Damage);
        }
    }
}
