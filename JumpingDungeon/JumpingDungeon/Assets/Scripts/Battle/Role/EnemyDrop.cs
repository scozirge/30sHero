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
        SkillLoot drops = DropSpawner.SpawnSkill(transform.position);
        drops.Init(DropSkill.SkillName, DropSkill.SkillDuration);
        PlayerRole pr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRole>();
        pr.InitMonsterSkill(DropSkill.SkillName, DropSkill);
    }

}
