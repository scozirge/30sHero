using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    [Tooltip("HPRecovery(Value:回血百分比) \nAvataEnergy(Time:秒) \nDamageBuff(Time:秒、Value:傷害百分比) \nImmortal(Time:秒) \nMove(Time:秒、Value:移動值)")]
    [SerializeField]
    List<LootData> LootList;
    [SerializeField]
    Image MyIcon;

    LootType Type;
    float Time;
    float Value;
    LootData Data;

    void Start()
    {
        int rand = Random.Range(0, LootList.Count);
        Data = LootList[rand];
        MyIcon.sprite = Data.LootIcon;
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(Data.GetParticle, Vector2.zero, Vector2.zero, _col.transform);
            _col.GetComponent<PlayerRole>().GetLoot(Data);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        BattleManage.RemoveLoot(this);
        Destroy(gameObject);
    }
}
