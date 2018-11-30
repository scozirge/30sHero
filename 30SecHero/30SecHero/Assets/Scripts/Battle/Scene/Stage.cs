using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    public int OccupyPlateSize;
    [HideInInspector]
    public int Floor;
    public void Init(int _floor)
    {
        Floor = _floor;
        MoveEnemyAndAmmoToCanvas();
    }
    void MoveEnemyAndAmmoToCanvas()
    {
        //把子彈移動到子彈物件底下
        GameObject ammoParent = GameObject.FindGameObjectWithTag("AmmoParent");
        if (ammoParent)
        {
            Ammo[] ammos = GetComponentsInChildren<Ammo>();
            //GetCom
            for (int i = 0; i < ammos.Length; i++)
            {
                ammos[i].transform.SetParent(ammoParent.transform);
            }
        }
        //把怪物移動到怪物物件底下
        GameObject enemyParent = GameObject.FindGameObjectWithTag("EnemyParent");
        if (enemyParent)
        {
            EnemyRole[] enemys = GetComponentsInChildren<EnemyRole>();
            for (int i = 0; i < enemys.Length; i++)
            {
                enemys[i].SetStemFromFloor(Floor);
                enemys[i].transform.SetParent(enemyParent.transform);
                enemys[i].gameObject.SetActive(false);
                BattleManage.AddEnemy(enemys[i]);
            }
        }
        //把戰利品移動到戰利品物件底下
        GameObject lootParent = GameObject.FindGameObjectWithTag("LootParent");
        if (lootParent)
        {
            Loot[] loots = GetComponentsInChildren<Loot>();
            for (int i = 0; i < loots.Length; i++)
            {
                loots[i].transform.SetParent(lootParent.transform);
            }
        }

    }
}
