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
    SkillLoot SkillLoot;

    static Loot LootPrefab;
    static SkillLoot SkillLootPrefab;
    static Transform MyTransfrom;
    static LootType Type;

    void Awake()
    {
        MyTransfrom = GetComponent<Transform>();
        LootPrefab = Loot;
        SkillLootPrefab = SkillLoot;
    }

    public static Loot SpawnLoot(Vector3 _pos)
    {
        Loot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as Loot;
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
