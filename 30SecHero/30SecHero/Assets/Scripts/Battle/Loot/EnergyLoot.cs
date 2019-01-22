using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AILootMove))]
public class EnergyLoot : Loot
{
    [SerializeField]
    ParticleSystem DeathEffect;
    [SerializeField]
    ParticleSystem TargetEffect;
    float Energy;


    public void Init(float _energy)
    {
        Energy = _energy;
    }
    void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == "AvatarTimer")
        {
            BattleManage.BM.MyPlayer.AddAvarTime(Energy);
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            if (TargetEffect) EffectEmitter.EmitParticle(TargetEffect, Vector3.zero, Vector3.zero, _col.gameObject.transform);
            AudioPlayer.PlaySound(GainSound);
            BattleManage.BM.MyPlayer.AddAvarTime(Energy);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
