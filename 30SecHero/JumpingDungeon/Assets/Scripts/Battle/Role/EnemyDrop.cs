using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public partial class EnemyRole
{
    [Tooltip("掉落裝備機率")]
    [SerializeField]
    float DropProbility;
    [Tooltip("最高掉落裝備數量")]
    [SerializeField]
    float DropCount;
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
        for (int i = 0; i < DropCount; i++)
        {
            if (ProbabilityGetter.GetResult(DropProbility))
            {
                DropSpawner.SpawnLoot(transform.position);
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
