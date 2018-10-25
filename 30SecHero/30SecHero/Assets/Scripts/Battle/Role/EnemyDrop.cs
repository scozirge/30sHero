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


    void Drop()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (!go)
            return;
        PlayerRole pr = go.GetComponent<PlayerRole>();
        //DropEquip
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
            if (Player.KillBossID.Contains(ID))
            {
                if (ProbabilityGetter.GetResult(GameSettingData.BossEmeraldProportion))
                {
                    ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
                    if (loot) loot.Init(ResourceType.Emerald, GameSettingData.BossEmerald + BattleManage.Floor);
                }
            }
            else
            {
                ResourceLoot loot = DropSpawner.SpawnResource(transform.position);
                if (loot) loot.Init(ResourceType.Emerald, GameSettingData.NewBossEmerald * BattleManage.Floor);
            }
        }
        //DropLoot;
        if (DesignateLoot.Count != 0)
        {
            for (int i = 0; i < DesignateLoot.Count; i++)
            {
                Loot loot = DropSpawner.SpawnLoot(transform.position);
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
            if (ProbabilityGetter.GetResult(DropSkillProbility + extraSkillProbility))
            {
                pr.InitMonsterSkill(DropSkill.PSkillName, DropSkill);
                SkillLoot drops = DropSpawner.SpawnSkill(transform.position);
                drops.Init(DropSkill.PSkillName);
            }
        }
        pr.GetExtraMoveSpeed();
    }

}
