using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLoot : Loot
{
    [SerializeField]
    Image MyIcon;
    [SerializeField]
    Sprite GoldIcon;
    [SerializeField]
    Sprite EmeraldIcon;
    [SerializeField]
    ParticleSystem DeathEffect;

    ResourceType MyType;
    int ResourceValue;
    public void Init(ResourceType _type, int _value)
    {
        MyType = _type;
        ResourceValue = _value;
        switch (MyType)
        {
            case ResourceType.Gold:
                MyIcon.sprite = GoldIcon;
                break;
            case ResourceType.Emerald:
                MyIcon.sprite = EmeraldIcon;
                break;
        }
        MyIcon.SetNativeSize();
    }


    void OnTriggerStay2D(Collider2D _col)
    {
        if (!ReadyToAcquire)
            return;
        if (_col.gameObject.tag == Force.Player.ToString())
        {
            _col.GetComponent<PlayerRole>().GetResource(MyType, ResourceValue);
            if (DeathEffect) EffectEmitter.EmitParticle(DeathEffect, transform.position, Vector3.zero, null);
            SelfDestroy();
        }
    }
    void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
