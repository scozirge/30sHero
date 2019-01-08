using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AILootMove))]
public class PatnerLoot : Loot
{
    [SerializeField]
    Transform PatnerTrans;
    [SerializeField]
    Image PatnerImage;
    [SerializeField]
    ParticleSystem DeathEffect;
    EnchantData MyData;

    int KillBossID;



    public void Init(int _bossID)
    {
        EnchantData ed = EnchantData.GetAvailableRandomEnchant();
        MyData = ed;
        PatnerImage.sprite = MyData.GetICON();
        PatnerImage.SetNativeSize();
        KillBossID = _bossID;
    }
    void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            BattleManage.KillBossAndGetEnchant(KillBossID, MyData);
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            AudioPlayer.PlaySound(GainSound);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
