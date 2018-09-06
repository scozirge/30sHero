using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class EnemyRole
{
    [SerializeField]
    float DropProbility;
    [SerializeField]
    float DropCount;

    void Drop()
    {
        for (int i = 0; i < DropCount; i++)
        {
            if (ProbabilityGetter.GetResult(DropProbility))
            {
                int rand = UnityEngine.Random.Range(0, Enum.GetNames(typeof(LootType)).Length);
                DropSpawner.SpawnLoot((LootType)rand, transform.position);
            }
        }
    }

}
