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
    protected override void Start()
    {
        base.Start();
        //牧羊人
        if (BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.Shepherd] > 0)
        {
            AILootMove alm = GetComponent<AILootMove>();
            if (alm != null)
                alm.AbsorbRadius = (int)(alm.AbsorbRadius * (1 + BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.Shepherd]));
        }
    }
    public void SetPic(string _spritePath)
    {
        if (SoulIcon != null || _spritePath == "")
        {
            SpritePath = _spritePath;
            Sprite sprite = Resources.Load<Sprite>(SpritePath);
            if (sprite!=null)
            {
                SoulIcon.sprite = sprite;
                SoulIcon.SetNativeSize();
            }
            else
            {
                Debug.LogWarning(string.Format("找不到Sprite路徑:{0}", SpritePath));
            }
        }
    }
    void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(GetEffect, transform.position, Vector3.zero, null);
            _col.GetComponent<PlayerRole>().GenerateMonsterSkill(Name, SpritePath);
            AudioPlayer.PlaySound(GainSound);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
