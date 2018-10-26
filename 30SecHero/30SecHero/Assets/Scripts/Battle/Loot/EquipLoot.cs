using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AILootMove))]
public class EquipLoot : Loot
{
    [SerializeField]
    Transform EquipTrans;
    [SerializeField]
    Image[] Icons;
    [SerializeField]
    ParticleSystem DeathEffect;
    EquipData MyData;



    public void Init(int _lv, int _quality)
    {
        MyData = EquipData.GetRandomNewEquip(_lv, _quality);
        for (int i = 0; i < Icons.Length; i++)
        {
            Icons[i].sprite = MyData.Icons[i];
            Icons[i].SetNativeSize();
        }
        EquipTrans.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-20, 20)));
    }
    void OnTriggerEnter2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            _col.GetComponent<PlayerRole>().GetEquip(MyData);
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
