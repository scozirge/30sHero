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
    [SerializeField]
    bool SpawnOnce;

    public static CameraController MyCameraControler;
    public static int Level;
    public static Vector2 ScreenSize;
    float SpawnIntervalTimer;
    int CurSpawnCount;
    float DestructMargin_Left;
    float DestructMargin_Right;
    int SpawnTimes;
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
        if (SpawnOnce && SpawnTimes > 0)
            return;
        for (int i = 0; i < Enemys.Count; i++)
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


        //Set SpawnPos
        int quadrant = 1;//象限
        int nearMargin = 0;//靠近左右邊(0)或靠近上下邊(1)
        er.transform.SetParent(EnemyParent);
        AIMove am = er.GetComponent<AIMove>();

        if (am != null)
        {
            Vector2 erScreenPos = am.Destination;

            if (erScreenPos == Vector2.zero)
            {
                erScreenPos = am.SetRandDestination();
            }

            if (erScreenPos.x >= 0 && erScreenPos.y >= 0)
            {
                quadrant = 1;//第1象限
                nearMargin = Mathf.Abs(ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
            else if (erScreenPos.x < 0 && erScreenPos.y >= 0)
            {
                quadrant = 2;//第2象限
                nearMargin = Mathf.Abs(-ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
            else if (erScreenPos.x < 0 && erScreenPos.y < 0)
            {
                quadrant = 3;//第3象限
                nearMargin = Mathf.Abs(-ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(-ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
            else if (erScreenPos.x > 0 && erScreenPos.y < 0)
            {
                quadrant = 4;//第4象限
                nearMargin = Mathf.Abs(ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(-ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
        }
        else
        {
            quadrant = Random.Range(1, 5);
        }
        Vector3 spawnPos = Vector3.zero;
        switch (quadrant)
        {
            case 1:
                spawnPos = (nearMargin == 0) ? new Vector3(ScreenSize.x / 2, Random.Range(0, ScreenSize.y / 2)) + MyCameraControler.transform.position :
                 new Vector3(Random.Range(0, ScreenSize.x / 2), ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
            case 2:
                spawnPos = (nearMargin == 0) ? new Vector3(-ScreenSize.x / 2, Random.Range(0, ScreenSize.y / 2)) + MyCameraControler.transform.position :
                new Vector3(Random.Range(-ScreenSize.x / 2, 0), ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
            case 3:
                spawnPos = (nearMargin == 0) ? new Vector3(-ScreenSize.x / 2, Random.Range(-ScreenSize.y / 2, 0)) + MyCameraControler.transform.position :
                new Vector3(Random.Range(-ScreenSize.x / 2, 0), -ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
            case 4:
                spawnPos = (nearMargin == 0) ? new Vector3(ScreenSize.x / 2, Random.Range(-ScreenSize.y / 2, 0)) + MyCameraControler.transform.position :
                new Vector3(Random.Range(0, ScreenSize.x / 2), -ScreenSize.y / 2) + MyCameraControler.transform.position;
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
        SpawnTimes++;
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
