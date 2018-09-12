using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public partial class EnemyRole
{
    [SerializeField]
    float DropProbility;
    [SerializeField]
    float DropCount;
    [SerializeField]
    Skill DropSkill;


    void Drop()
    {
        PlayerRole pr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRole>();
        //DropLoot;
        for (int i = 0; i < DropCount; i++)
        {
            if (ProbabilityGetter.GetResult(DropProbility))
            {
                int rand = UnityEngine.Random.Range(0, Enum.GetNames(typeof(LootType)).Length);
                DropSpawner.SpawnLoot((LootType)rand, transform.position);
            }
        }

        //DropSkill
        if (DropSkill)
        {
            pr.InitMonsterSkill(DropSkill.PSkillName, DropSkill);
            SkillLoot drops = DropSpawner.SpawnSkill(transform.position);
            drops.Init(DropSkill.PSkillName);

        }
        pr.GetExtraMoveSpeed();
    }

}
