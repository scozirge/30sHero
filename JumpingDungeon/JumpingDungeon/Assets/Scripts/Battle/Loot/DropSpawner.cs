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
    [SerializeField]
    SkillLoot SkillLoot;

    static Loot LootPrefab;
    static SkillLoot SkillLootPrefab;
    static Transform MyTransfrom;
    static LootType Type;
    static bool IsDesignate;

    void Awake()
    {
        MyTransfrom = GetComponent<Transform>();
        LootPrefab = Loot;
        SkillLootPrefab = SkillLoot;
        Type = DesignateType;
        IsDesignate = Designate;
    }

    public static Loot SpawnLoot(LootType _type, Vector3 _pos)
    {
        if (!IsDesignate)
            Type = _type;
        Loot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as Loot;
        loot.Init(Type);
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
        return loot;
    }
    public static SkillLoot SpawnSkill(Vector3 _pos)
    {
        SkillLoot loot = Instantiate(SkillLootPrefab, Vector3.zero, Quaternion.identity) as SkillLoot;
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
        return loot;
    }
}
