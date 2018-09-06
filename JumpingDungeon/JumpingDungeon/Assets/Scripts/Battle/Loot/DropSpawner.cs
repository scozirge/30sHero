using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField]
    Loot Loot;
    [SerializeField]
    bool Designate;
    [SerializeField]
    LootType DesignateType;

    static Loot LootPrefab;
    static Transform MyTransfrom;
    static LootType Type;
    static bool IsDesignate;

    void Awake()
    {
        MyTransfrom = GetComponent<Transform>();
        LootPrefab = Loot;
        Type = DesignateType;
        IsDesignate = Designate;
    }

    public static void SpawnLoot(LootType _type,Vector3 _pos)
    {
        if (!IsDesignate)
            Type = _type;
        Debug.Log(Type);
        Loot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as Loot;
        loot.Init(Type);
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
    }

}
