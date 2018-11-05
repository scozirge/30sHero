using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillLoot : Loot
{
    [SerializeField]
    Image SoulIcon;
    [SerializeField]
    ParticleSystem ExistEffect;
    [SerializeField]
    ParticleSystem GetEffect;

    public string Name { get; private set; }
    string SpritePath;

    public void Init(string _name)
    {
        Name = _name;
        EffectEmitter.EmitParticle(ExistEffect, Vector2.zero, Vector3.zero, transform);
    }
    public void SetPic(string _spritePath)
    {
        if (SoulIcon != null || _spritePath == "")
        {
            SpritePath = _spritePath;
            SoulIcon.sprite = Resources.Load<Sprite>(SpritePath);
            SoulIcon.SetNativeSize();
        }
    }
    void OnTriggerEnter2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(GetEffect, transform.position, Vector3.zero, null);
            _col.GetComponent<PlayerRole>().GenerateMonsterSkill(Name, SpritePath);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
