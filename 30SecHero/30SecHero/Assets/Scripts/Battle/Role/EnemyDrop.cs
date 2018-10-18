using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public partial class EnemyRole
{
    [Tooltip("掉落道具機率")]
    [SerializeField]
    float DropProbility;
    [Tooltip("指定掉落道具")]
    [SerializeField]
    List<LootType> DesignateLoot = new List<LootType>();
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
        //DropLoot;
        for (int i = 0; i < DesignateLoot.Count; i++)
        {
            if (DesignateLoot[i] == null)
                continue;
            if (ProbabilityGetter.GetResult(DropProbility))
            {
                Loot loot = DropSpawner.SpawnLoot(transform.position);
                if (loot) loot.DesignateLoot(DesignateLoot[i]);
            }
        }

        //DropSkill
        if (DropSkill)
        {
            if (ProbabilityGetter.GetResult(DropSkillProbility))
            {
                pr.InitMonsterSkill(DropSkill.PSkillName, DropSkill);
                SkillLoot drops = DropSpawner.SpawnSkill(transform.position);
                drops.Init(DropSkill.PSkillName);
            }
        }
        pr.GetExtraMoveSpeed();
    }

}
