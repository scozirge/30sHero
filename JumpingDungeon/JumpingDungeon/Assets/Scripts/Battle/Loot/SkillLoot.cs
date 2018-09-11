using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLoot : MonoBehaviour {

    [SerializeField]
    ParticleSystem GetEffect;
    float SkillDuration;

    public string Name { get; private set; }

    public void Init(string _name,float _skillDuration)
    {
        Name = _name;
        SkillDuration = _skillDuration;
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(GetEffect, transform.position, Vector3.zero, null);
            _col.GetComponent<PlayerRole>().GenerateMonsterSkill(Name, SkillDuration);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
