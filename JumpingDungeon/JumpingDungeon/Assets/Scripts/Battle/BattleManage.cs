using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManage : MonoBehaviour
{
    [SerializeField]
    float SpawnInterval;
    [SerializeField]
    int SpawnCount;
    [SerializeField]
    float SpawnCountInterval;
    [SerializeField]
    List<EnemyRole> Enemys;
    [SerializeField]
    Transform EnemyParent;
    [SerializeField]
    CameraController CameraControler;
    [SerializeField]
    EnemyRole DesignatedEnemy;

    public static CameraController MyCameraControler;
    public static int Level;
    public static Vector2 ScreenSize;
    float SpawnIntervalTimer;
    int CurSpawnCount;
    float DestructMargin_Left;
    float DestructMargin_Right;
    List<EnemyRole> EnemyList = new List<EnemyRole>();

    // Use this for initialization
    void Start()
    {
        SpawnIntervalTimer = SpawnInterval;
        MyCameraControler = CameraControler;
        CurSpawnCount = 0;
        ScreenSize = MyCameraControler.ScreenSize;

    }
    void SpawnIntervalTimerFunc()
    {
        if (SpawnInterval <= 0)
            return;
        if (SpawnIntervalTimer > 0)
            SpawnIntervalTimer -= Time.deltaTime;
        else
        {
            SpawnIntervalTimer = SpawnInterval;
            SpanwEnemy();
        }
    }
    void SpanwEnemy()
    {
        for(int i=0;i<Enemys.Count;i++)
        {
            if (Enemys[i] == null)
                Enemys.RemoveAt(i);
        }
        int rndEnemy = Random.Range(0, Enemys.Count);
        EnemyRole er;
        if (DesignatedEnemy)
            er = Instantiate(DesignatedEnemy, Vector3.zero, Quaternion.identity) as EnemyRole;
        else
            er = Instantiate(Enemys[rndEnemy], Vector3.zero, Quaternion.identity) as EnemyRole;
        er.transform.SetParent(EnemyParent);
        int randDir = Random.Range(0, 3);
        Vector3 spawnPos = Vector3.zero;
        switch (randDir)
        {
            case 0:
                spawnPos = new Vector3(ScreenSize.x / 2 + 10, Random.Range(-ScreenSize.y / 2, ScreenSize.y / 2)) + MyCameraControler.transform.position;
                break;
            case 1:
                spawnPos = new Vector3(Random.Range(0, ScreenSize.x / 2), -ScreenSize.y / 2 - 10) + MyCameraControler.transform.position;
                break;
            case 2:
                spawnPos = new Vector3(ScreenSize.x / 2 + 10, Random.Range(-ScreenSize.y / 2, ScreenSize.y / 2)) + MyCameraControler.transform.position;
                //spawnPos = new Vector3(-ScreenSize.x / 2 - 10, Random.Range(-ScreenSize.y / 2, ScreenSize.y / 2)) + CC.transform.position;
                break;
            case 3:
                spawnPos = new Vector3(Random.Range(0, ScreenSize.x / 2), ScreenSize.y / 2 + 10) + MyCameraControler.transform.position;
                break;
        }
        spawnPos.z = 0;
        er.transform.position = spawnPos;
        CurSpawnCount++;
        if (CurSpawnCount < SpawnCount)
            StartCoroutine(WaitToSpawnEnemy());
        else
            CurSpawnCount = 0;
        EnemyList.Add(er);
    }
    IEnumerator WaitToSpawnEnemy()
    {
        yield return new WaitForSeconds(SpawnCountInterval);
        SpanwEnemy();
    }
    // Update is called once per frame
    void Update()
    {
        SpawnIntervalTimerFunc();
        InActivityOutSideEnemys();
    }
    void InActivityOutSideEnemys()
    {
        DestructMargin_Left = (MyCameraControler.transform.position.x - (ScreenSize.x / 2 + 200));
        DestructMargin_Right = (MyCameraControler.transform.position.x + (ScreenSize.x / 2 + 200));
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null)
                EnemyList.RemoveAt(i);
            else
            {
                if (EnemyList[i].transform.position.x < DestructMargin_Left ||
                    EnemyList[i].transform.position.x > DestructMargin_Right)
                {
                    EnemyList[i].gameObject.SetActive(false);
                }
                else
                    EnemyList[i].gameObject.SetActive(true);
            }
        }
    }
}
