using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionLoot : Loot
{
    [Tooltip("指定要出的藥水類型")]
    [SerializeField]
    LootType DesginateLootType;
    [Tooltip("指定出藥水")]
    [SerializeField]
    bool Designate = false;
    [Tooltip("HPRecovery(Value:回血百分比) \nAvataEnergy(Time:秒) \nDamageBuff(Time:秒、Value:傷害百分比) \nImmortal(Time:秒) \nMove(Time:秒、Value:移動值)")]
    [SerializeField]
    List<LootData> LootList;
    [SerializeField]
    Image MyIcon;
    [SerializeField]
    ParticleSystem DeathEffect;

    float Time;
    float Value;
    LootData Data;
    bool IsDesignateLoot;

    public void DesignateLoot(LootType _type)
    {
        for (int i = 0; i < LootList.Count; i++)
        {
            if (LootList[i].Type == _type)
            {
                Data = LootList[i];
                IsDesignateLoot = true;
                return;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        if (!IsDesignateLoot)
        {
            if (Designate)
            {
                DesignateLoot(DesginateLootType);
            }
            else
            {
                int rand = Random.Range(0, LootList.Count);
                Data = LootList[rand];
            }
        }
        MyIcon.sprite = Data.LootIcon;
        MyIcon.SetNativeSize();
        //藥水蒐集者
        if (BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.Collector] > 0)
        {
            AILootMove alm = GetComponent<AILootMove>();
            if (alm != null)
                alm.AbsorbRadius = (int)(alm.AbsorbRadius * (1 + BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.Collector]));
        }
    }

    void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(Data.GetParticle, Vector2.zero, Vector2.zero, _col.transform);
            _col.GetComponent<PlayerRole>().GetLoot(Data);
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            AudioPlayer.PlaySound(GainSound);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        BattleManage.RemoveLoot(this);
        Destroy(gameObject);
    }
}
