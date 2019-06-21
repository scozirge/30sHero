using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public partial class EnemyRole
{
    [Tooltip("掉落裝備數量")]
    [SerializeField]
    int DropEquipCount = 1;
    [Tooltip("掉落金幣數量")]
    [SerializeField]
    int DropGoldCount = 1;
    [Tooltip("掉落道具數量")]
    [SerializeField]
    int DropLootCount = 1;
    [Tooltip("指定掉落道具")]
    [SerializeField]
    List<LootType> DesignateLoot;
    [Tooltip("掉落技能機率")]
    [SerializeField]
    float DropSkillProbility;
    [Tooltip("掉落技能(從自己身上拉一個技能當作掉落技能)")]
    [SerializeField]
    Skill DropSkill;

    public void ExtralGoldDrop()
    {
        ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
        if (loot) loot.Init(ResourceType.Gold, GameSettingData.GetEnemyDropGold(BattleManage.Floor));
    }
    public void ExtraEmeralddDrop()
    {
        ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
        if (loot) loot.Init(ResourceType.Emerald, GameSettingData.BossEmerald);
    }
    void Drop()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (!go)
            return;
        PlayerRole pr = go.GetComponent<PlayerRole>();
        //DropEquip
        if (Type == EnemyType.Demogorgon)
        {
            if (ID == 15 && !Player.KillBossID.Contains(ID))
            {
                int equipQuality = 5;
                EquipLoot loot = DropSpawner.SpawnEquip(transform.position);
                if (loot) loot.Init(BattleManage.Floor, equipQuality, EquipType.Weapon);
            }
            for (int i = 0; i < 3; i++)
            {
                int equipQuality = GameSettingData.GetBossRandomEquipQuality();
                if (equipQuality != 0)//0代表隨機到沒有掉落裝備
                {
                    EquipLoot loot = DropSpawner.SpawnEquip(transform.position);
                    if (loot) loot.Init(BattleManage.Floor, equipQuality);
                }
            }
        }
        else
        {
            int extraWeight = (BattleManage.BM.MyPlayer != null) ? BattleManage.BM.MyPlayer.EquipDropWeight : 0;
            for (int i = 0; i < DropEquipCount; i++)
            {
                int equipQuality = GameSettingData.GetRandomEquipQuality(extraWeight);
                if (equipQuality != 0)//0代表隨機到沒有掉落裝備
                {
                    EquipLoot loot = DropSpawner.SpawnEquip(transform.position);
                    if (loot) loot.Init(BattleManage.Floor, equipQuality);
                }
            }
        }

        //DropGold
        for (int i = 0; i < DropGoldCount; i++)
        {
            if (ProbabilityGetter.GetResult(GameSettingData.EnemyDropGoldProportion))
            {
                ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
                if (loot) loot.Init(ResourceType.Gold, GameSettingData.GetEnemyDropGold(BattleManage.Floor));
            }
        }
        //DropEmerald
        if (Type == EnemyType.Demogorgon)
        {
            //弒王者(擊殺BOSS有機率獲得寶石)
            if (ProbabilityGetter.GetResult(BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.KingKiller]))
            {
                ExtraEmeralddDrop();
            }
            if (Player.KillBossID.Contains(ID))
            {
                if (ProbabilityGetter.GetResult(GameSettingData.BossEmeraldProportion))
                {
                    ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
                    if (loot) loot.Init(ResourceType.Emerald, GameSettingData.BossEmerald);
                }
            }
            else//擊殺尚未擊殺過的新BOSS
            {
                //寶石
                ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
                if (loot) loot.Init(ResourceType.Emerald, GameSettingData.NewBossEmerald);
            }
            //解鎖夥伴並加入已擊殺BOSS清單
            if(Player.KillBossID.Contains(15))
            {
                EnchantData ed = EnchantData.GetUnLockRandomEnchant();
                if (ed != null)
                {
                    PatnerLoot ploot = DropSpawner.SpawnPatner(transform.position);
                    BattleManage.KillBossAndGetEnchant(ID, ed);
                    if (ploot) ploot.Init(ID, ed);
                }
            }
            else
            {
                EnchantData ed = EnchantData.GetCertainEnchant(20);
                if (ed != null)
                {
                    PatnerLoot ploot = DropSpawner.SpawnPatner(transform.position);
                    BattleManage.KillBossAndGetEnchant(ID, ed);
                    if (ploot) ploot.Init(ID, ed);
                }
            }
        }
        //DropLoot;
        if (DesignateLoot.Count != 0)
        {
            for (int i = 0; i < DesignateLoot.Count; i++)
            {
                PotionLoot loot = DropSpawner.SpawnLoot(transform.position);
                if (loot) loot.DesignateLoot(DesignateLoot[i]);
            }
        }
        else
        {
            float extraPotionProbility = (BattleManage.BM.MyPlayer != null) ? BattleManage.BM.MyPlayer.PotionDrop : 0;
            for (int i = 0; i < DropLootCount; i++)
            {
                if (ProbabilityGetter.GetResult(GameSettingData.EnemyDropPotionProportion + extraPotionProbility))
                {
                    DropSpawner.SpawnLoot(transform.position);
                }
            }
        }
        //DropSkill
        if (DropSkill)
        {
            float extraSkillProbility = (BattleManage.BM.MyPlayer != null) ? BattleManage.BM.MyPlayer.SkillDrop : 0;
            extraSkillProbility += BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.Harvester] * BattleManage.BM.MyPlayer.ActiveMonsterSkillCount;
            if (ProbabilityGetter.GetResult(DropSkillProbility + extraSkillProbility))
            {
                pr.InitMonsterSkill(DropSkill.PSkillName, DropSkill);
                SkillLoot drops = DropSpawner.SpawnSkill(transform.position);
                drops.Init(DropSkill.PSkillName);
                drops.SetPic(GetSkillLootSpritePath());

            }
        }
        pr.GetExtraMoveSpeed();
        //DropEnergy
        if(KillAvatarTime>0)
        {
            EnergyLoot el= DropSpawner.SpawnEnergy(transform.position);
            if (el) el.Init(KillAvatarTime);
        }
    }
    string GetSkillLootSpritePath()
    {
        string spriteName = RoleImg.sprite.name.TrimEnd("_r".ToCharArray());
        spriteName = spriteName.TrimEnd("_a".ToCharArray());
        spriteName = spriteName.TrimStart("char_e".ToCharArray());
        spriteName = "mask_" + spriteName;
        spriteName = string.Format(GameSettingData.SoulPath, spriteName);
        return spriteName;
    }
}
