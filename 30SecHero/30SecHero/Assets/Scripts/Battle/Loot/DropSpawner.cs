using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField]
    EquipLoot Equip;
    [SerializeField]
    ResourceLoot Resource;
    [SerializeField]
    PotionLoot Loot;
    [SerializeField]
    bool Designate;
    [SerializeField]
    SkillLoot SkillLoot;
    [SerializeField]
    PatnerLoot PatnerLoot;
    [SerializeField]
    EnergyLoot EnergyLoot;

    static PotionLoot LootPrefab;
    static SkillLoot SkillLootPrefab;
    static ResourceLoot ResourcePrefab;
    static EquipLoot EquipPrefab;
    static PatnerLoot PatnerPrefab;
    static EnergyLoot EnergyPrefab;
    static Transform MyTransfrom;
    static LootType Type;

    void Awake()
    {
        MyTransfrom = GetComponent<Transform>();
        LootPrefab = Loot;
        SkillLootPrefab = SkillLoot;
        ResourcePrefab = Resource;
        EquipPrefab = Equip;
        PatnerPrefab = PatnerLoot;
        EnergyPrefab = EnergyLoot;
    }
    public static EquipLoot SpawnEquip(Vector3 _pos)
    {
        EquipLoot loot = Instantiate(EquipPrefab, Vector3.zero, Quaternion.identity) as EquipLoot;
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
        return loot;
    }
    public static ResourceLoot SpawnResource(Vector3 _pos)
    {
        ResourceLoot loot = Instantiate(ResourcePrefab, Vector3.zero, Quaternion.identity) as ResourceLoot;
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
        return loot;
    }
    public static PotionLoot SpawnLoot(Vector3 _pos)
    {
        PotionLoot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as PotionLoot;
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
    public static PatnerLoot SpawnPatner(Vector3 _pos)
    {
        PatnerLoot loot = Instantiate(PatnerPrefab, Vector3.zero, Quaternion.identity) as PatnerLoot;
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
        return loot;
    }
    public static EnergyLoot SpawnEnergy(Vector3 _pos)
    {
        EnergyLoot loot = Instantiate(EnergyPrefab, Vector3.zero, Quaternion.identity) as EnergyLoot;
        loot.transform.SetParent(MyTransfrom);
        loot.transform.localPosition = _pos;
        return loot;
    }
}
