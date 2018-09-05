using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField]
    float AvataTime;
    [SerializeField]
    float DamageBuff;
    [SerializeField]
    float DamageBuffTime;
    [SerializeField]
    float InvincibleTime;
    [SerializeField]
    int HPRecovery;
    [SerializeField]
    int Money;
    [SerializeField]
    float MoneyLevelBuff;
    [SerializeField]
    ParticleSystem GetEffect;

    [SerializeField]
    LootType Type;
    float Time;
    float Value;
    BufferData Data;

    public void Init(LootType _type)
    {
        Type = _type;
        switch (_type)
        {
            case LootType.AvataEnergy:
                Time = AvataTime;
                break;
            case LootType.DamageBuff:
                Value = DamageBuff;
                Time = DamageBuffTime;
                break;
            case LootType.Euipment:
                break;
            case LootType.HPRecovery:
                Value = HPRecovery;
                break;
            case LootType.InvinciblePotion:
                Time = InvincibleTime;
                break;
            case LootType.Money:
                Value = Money + MoneyLevelBuff * BattleManage.Level;
                break;
        }
        Data = new BufferData(Time, Value);
    }

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            EffectEmitter.EmitParticle(GetEffect, transform.position, Vector3.zero, null);
            _col.GetComponent<PlayerRole>().GetLoot(Type, Data);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
