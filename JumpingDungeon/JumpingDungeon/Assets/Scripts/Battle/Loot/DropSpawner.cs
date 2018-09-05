using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField]
    Loot Loot;

    static Loot LootPrefab;
    static Transform MyTransfrom;
    LootType Type;

    void Awake()
    {
        MyTransfrom = GetComponent<Transform>();
        LootPrefab = Loot;
    }

    public static void SpawnLoot(LootType _type,Vector3 _pos)
    {
        Loot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as Loot;
        loot.Init(_type);
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
    }

}
