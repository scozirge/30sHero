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
    EnemyRole[] Enemys;
    [SerializeField]
    Transform EnemyParent;
    [SerializeField]
    CameraController CC;

    public static int Level;
    Vector2 ScreenSize;
    float SpawnIntervalTimer;
    int CurSpawnCount;

    // Use this for initialization
    void Start()
    {
        SpawnIntervalTimer = SpawnInterval;
        CurSpawnCount = 0;
        ScreenSize = CC.ScreenSize;
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
        int rndEnemy = Random.Range(0, Enemys.Length);
        EnemyRole er = Instantiate(Enemys[rndEnemy], Vector3.zero, Quaternion.identity) as EnemyRole;
        er.transform.SetParent(EnemyParent);
        int randDir = Random.Range(0, 3);
        Vector3 spawnPos = Vector3.zero;
        switch (randDir)
        {
            case 0:
                spawnPos = new Vector3(ScreenSize.x / 2 + 10, Random.Range(-ScreenSize.y / 2, ScreenSize.y / 2)) + CC.transform.position;
                break;
            case 1:
                spawnPos = new Vector3(Random.Range(0, ScreenSize.x / 2), -ScreenSize.y / 2 - 10) + CC.transform.position;
                break;
            case 2:
                spawnPos = new Vector3(ScreenSize.x / 2 + 10, Random.Range(-ScreenSize.y / 2, ScreenSize.y / 2)) + CC.transform.position;
                //spawnPos = new Vector3(-ScreenSize.x / 2 - 10, Random.Range(-ScreenSize.y / 2, ScreenSize.y / 2)) + CC.transform.position;
                break;
            case 3:
                spawnPos = new Vector3(Random.Range(0, ScreenSize.x / 2), ScreenSize.y / 2 + 10) + CC.transform.position;
                break;
        }
        spawnPos.z = 0;
        er.transform.position = spawnPos;
        CurSpawnCount++;
        if (CurSpawnCount < SpawnCount)
            WaitToSpawnEnemy();
        else
            CurSpawnCount = 0;
    }
    IEnumerable WaitToSpawnEnemy()
    {
        yield return new WaitForSeconds(SpawnCountInterval);
        SpanwEnemy();
    }
    // Update is called once per frame
    void Update()
    {
        SpawnIntervalTimerFunc();
    }
}
