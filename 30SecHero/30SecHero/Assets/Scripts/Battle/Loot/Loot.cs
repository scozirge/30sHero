using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    [Tooltip("指定要出第幾個道具")]
    [SerializeField]
    int DesginateLootIndex;
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

    void Start()
    {
        int rand = 0;
        if (DesginateLootIndex == 0)
            rand = Random.Range(0, LootList.Count);
        else
            rand = DesginateLootIndex - 1;
        if (rand >= LootList.Count)
        {
            rand = 0;
            Debug.LogWarning("輸入的指定寶物超出索引值");
        }
        Data = LootList[rand];
        MyIcon.sprite = Data.LootIcon;
        MyIcon.SetNativeSize();
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(Data.GetParticle, Vector2.zero, Vector2.zero, _col.transform);
            _col.GetComponent<PlayerRole>().GetLoot(Data);
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        BattleManage.RemoveLoot(this);
        Destroy(gameObject);
    }
}
