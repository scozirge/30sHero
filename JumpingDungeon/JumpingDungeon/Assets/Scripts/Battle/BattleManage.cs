using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManage : MonoBehaviour
{
    [SerializeField]
    bool TestMode;
    [SerializeField]
    float SpawnEnemyInterval;
    [SerializeField]
    float SpawnLootInterval;
    [SerializeField]
    int SpawnEnemyCount;
    [SerializeField]
    int SpawnLootCount;
    [SerializeField]
    float SpawnEnemyCountInterval;
    [SerializeField]
    float SpawnLootCountInterval;
    [SerializeField]
    int MaxEnemyCout;
    [SerializeField]
    int MaxLootCout;
    [SerializeField]
    List<EnemyRole> Enemys;
    [SerializeField]
    EnemyRole DesignatedEnemy;
    [SerializeField]
    Loot LootPrefab;
    [SerializeField]
    CameraController CameraControler;
    [SerializeField]
    PlayerRole MyPlayer;


    static List<EnemyRole> AvailableMillions;
    static List<EnemyRole> AvailableDemonGergons;
    int CurSpawnCount;
    int CurSpawnLootCount;
    Transform EnemyParent;
    Transform LootParetn;
    MyTimer SpawnEnemyTimer;
    MyTimer SpawnLootTimer;
    public static BattleManage BM;
    public static CameraController MyCameraControler;
    public static int Floor;
    public static Vector2 ScreenSize;
    float DestructMargin_Left;
    float DestructMargin_Right;
    List<EnemyRole> EnemyList = new List<EnemyRole>();
    List<Loot> LootList = new List<Loot>();
    static int NextDemogorgonFloor;
    static bool IsDemogorgonFloor;

    // Use this for initialization
    void Awake()
    {
        if (!GameManager.IsInit)
            GameManager.DeployGameManager();

        BM = this;
        EnemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<Transform>();
        LootParetn = GameObject.FindGameObjectWithTag("LootParent").GetComponent<Transform>();
        InitStage();
        SpawnEnemyTimer = new MyTimer(SpawnEnemyInterval, SpanwEnemy, true, false);
        SpawnLootTimer = new MyTimer(SpawnLootInterval, SpawnLoot, true, false);
        MyCameraControler = CameraControler;
        CurSpawnCount = 0;
        ScreenSize = MyCameraControler.ScreenSize;
        //SpawnEnemySet
        if (TestMode)
        {
            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i] == null)
                    Enemys.RemoveAt(i);
            }
            AvailableMillions = Enemys;
        }
        else
            AvailableMillions = EnemyData.GetAvailableMillions(Floor);
        AvailableDemonGergons = EnemyData.GetNextDemogorgon(Floor, out NextDemogorgonFloor);
        IsDemogorgonFloor = CheckDemogorgon(Floor);
    }

    void SpawnDemogorgon()
    {
        for (int i = 0; i < AvailableDemonGergons.Count; i++)
        {
            EnemyRole er = Instantiate(AvailableDemonGergons[i], Vector3.zero, Quaternion.identity) as EnemyRole;
            //Set SpawnPos
            int quadrant = 1;//象限
            int nearMargin = 0;//靠近左右邊(0)或靠近上下邊(1)
            er.transform.SetParent(EnemyParent);
            AIMove am = er.GetComponent<AIRoleMove>();
            SetQuadrantAndNearMargin(am, ref quadrant, ref nearMargin);
            er.transform.position = GetSpawnPos(quadrant, nearMargin);
            EnemyList.Add(er);
        }
        AvailableDemonGergons = EnemyData.GetNextDemogorgon(Floor+1, out NextDemogorgonFloor);
        IsDemogorgonFloor = false;
    }
    void SpanwEnemy()
    {
        if (!CheckEnemySpawnLimit())
            return;
        int rndEnemy = Random.Range(0, AvailableMillions.Count);
        EnemyRole er;
        if (DesignatedEnemy)
            er = Instantiate(DesignatedEnemy, Vector3.zero, Quaternion.identity) as EnemyRole;
        else
            er = Instantiate(AvailableMillions[rndEnemy], Vector3.zero, Quaternion.identity) as EnemyRole;


        //Set SpawnPos
        int quadrant = 1;//象限
        int nearMargin = 0;//靠近左右邊(0)或靠近上下邊(1)
        er.transform.SetParent(EnemyParent);
        AIMove am = er.GetComponent<AIRoleMove>();
        SetQuadrantAndNearMargin(am, ref quadrant, ref nearMargin);
        er.transform.position = GetSpawnPos(quadrant, nearMargin);
        CurSpawnCount++;
        if (CurSpawnCount < SpawnEnemyCount)
            StartCoroutine(WaitToSpawnEnemy());
        else
        {
            CurSpawnCount = 0;
            SpawnEnemyTimer.StartRunTimer = true;
        }
        EnemyList.Add(er);
    }
    bool CheckEnemySpawnLimit()
    {
        if (MaxEnemyCout == 0)
            return true;
        if (EnemyList.Count < MaxEnemyCout)
            return true;
        return false;
    }
    bool CheckLootSpawnLimit()
    {
        if (MaxLootCout == 0)
            return true;
        if (LootList.Count < MaxLootCout)
            return true;
        return false;
    }
    void SpawnLoot()
    {
        if (!CheckLootSpawnLimit())
            return;
        Loot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as Loot;

        //Set SpawnPos
        int quadrant = 1;//象限
        int nearMargin = 0;//靠近左右邊(0)或靠近上下邊(1)
        loot.transform.SetParent(LootParetn);
        AIMove am = loot.GetComponent<AILootMove>();
        SetQuadrantAndNearMargin(am, ref quadrant, ref nearMargin);
        loot.transform.position = GetSpawnPos(quadrant, nearMargin);
        CurSpawnLootCount++;
        if (CurSpawnLootCount < SpawnLootCount)
            StartCoroutine(WaitToSpawnLoot());
        else
        {
            CurSpawnLootCount = 0;
            SpawnLootTimer.StartRunTimer = true;
        }
        LootList.Add(loot);
    }
    void SetQuadrantAndNearMargin(AIMove _am, ref int _quadrant, ref int _nearMargin)
    {
        if (_am != null)
        {
            Vector2 erScreenPos = _am.Destination;
            if (erScreenPos == Vector2.zero)
            {
                erScreenPos = _am.SetRandDestination();
            }

            if (erScreenPos.x >= 0 && erScreenPos.y >= 0)
            {
                _quadrant = 1;//第1象限
                _nearMargin = Mathf.Abs(ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
            else if (erScreenPos.x < 0 && erScreenPos.y >= 0)
            {
                _quadrant = 2;//第2象限
                _nearMargin = Mathf.Abs(-ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
            else if (erScreenPos.x < 0 && erScreenPos.y < 0)
            {
                _quadrant = 3;//第3象限
                _nearMargin = Mathf.Abs(-ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(-ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
            else if (erScreenPos.x > 0 && erScreenPos.y < 0)
            {
                _quadrant = 4;//第4象限
                _nearMargin = Mathf.Abs(ScreenSize.x / 2 - erScreenPos.x) < Mathf.Abs(-ScreenSize.y / 2 - erScreenPos.y) ? 0 : 1;
            }
        }
        else
        {
            _quadrant = Random.Range(1, 5);
        }
    }
    Vector3 GetSpawnPos(int _quadrant, int _nearMargin)
    {
        Vector3 spawnPos = Vector3.zero;
        switch (_quadrant)
        {
            case 1:
                spawnPos = (_nearMargin == 0) ? new Vector3(ScreenSize.x / 2, Random.Range(0, ScreenSize.y / 2)) + MyCameraControler.transform.position :
                 new Vector3(Random.Range(0, ScreenSize.x / 2), ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
            case 2:
                spawnPos = (_nearMargin == 0) ? new Vector3(-ScreenSize.x / 2, Random.Range(0, ScreenSize.y / 2)) + MyCameraControler.transform.position :
                new Vector3(Random.Range(-ScreenSize.x / 2, 0), ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
            case 3:
                spawnPos = (_nearMargin == 0) ? new Vector3(-ScreenSize.x / 2, Random.Range(-ScreenSize.y / 2, 0)) + MyCameraControler.transform.position :
                new Vector3(Random.Range(-ScreenSize.x / 2, 0), -ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
            case 4:
                spawnPos = (_nearMargin == 0) ? new Vector3(ScreenSize.x / 2, Random.Range(-ScreenSize.y / 2, 0)) + MyCameraControler.transform.position :
                new Vector3(Random.Range(0, ScreenSize.x / 2), -ScreenSize.y / 2) + MyCameraControler.transform.position;
                break;
        }
        spawnPos.z = 0;
        return spawnPos;
    }
    IEnumerator WaitToSpawnEnemy()
    {
        yield return new WaitForSeconds(SpawnEnemyCountInterval);
        SpanwEnemy();
    }
    IEnumerator WaitToSpawnLoot()
    {
        yield return new WaitForSeconds(SpawnLootCountInterval);
        SpawnLoot();
    }
    public static void RemoveEnemy(EnemyRole _er)
    {
        BM.EnemyList.Remove(_er);
    }
    public static void RemoveLoot(Loot _loot)
    {
        BM.LootList.Remove(_loot);
    }
    // Update is called once per frame
    void Update()
    {
        InActivityOutSideEnemys();
        UpdateCurPlate();
        SpawnEnemyTimer.RunTimer();
        SpawnLootTimer.RunTimer();
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
