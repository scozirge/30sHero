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
        GameObject enemyParent = GameObject.FindGameObjectWithTag("EnemyParent");

        if (enemyParent)
        {
            EnemyRole[] enemys = GetComponentsInChildren<EnemyRole>();
            for (int i = 0; i < enemys.Length; i++)
            {
                enemys[i].transform.SetParent(enemyParent.transform);
                enemys[i].gameObject.SetActive(false);
                BattleManage.AddEnemy(enemys[i]);
            }
        }

    }
}
