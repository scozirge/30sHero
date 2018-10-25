using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManage : MonoBehaviour
{
    [SerializeField]
    bool TestMode;
    [SerializeField]
    float EnemyFirstHalfInterval;
    [SerializeField]
    float EnemySecondHalfInterval;
    [SerializeField]
    float PotionInterval;
    [SerializeField]
    int EnemyFirstHalfMinCount;
    [SerializeField]
    int EnemyFirstHalfMaxCount;
    [SerializeField]
    int EnemySecondHalfMinCount;
    [SerializeField]
    int EnemySecondHalfMaxCount;
    [SerializeField]
    float EnemySpawnInterval;
    [SerializeField]
    int MaxEnemy;
    [SerializeField]
    int MaxLoot;
    [SerializeField]
    List<EnemyRole> Enemys;
    [SerializeField]
    EnemyRole DesignatedEnemy;
    [SerializeField]
    Loot LootPrefab;
    [SerializeField]
    CameraController CameraControler;
    [SerializeField]
    public PlayerRole MyPlayer;
    [SerializeField]
    GameObject SceneObject;

    static List<EnemyRole> AvailableMillions;
    static List<EnemyRole> AvailableDemonGergons;
    int CurSpawnCount;
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
    bool IsInit;
    int EnemySpawnCount;
    public static int EnemyKill;
    

    // Use this for initialization
    void Awake()
    {
        if (!GameManager.IsInit)
            GameManager.DeployGameManager();
        SceneObject.SetActive(false);
    }
    void Init()
    {
        SceneObject.SetActive(true);
        InitSettlement();
        InitBattleSetting();
        BM = this;
        EnemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<Transform>();
        LootParetn = GameObject.FindGameObjectWithTag("LootParent").GetComponent<Transform>();
        InitStage();
        MyCameraControler = CameraControler;
        CurSpawnCount = 0;
        EnemyKill = 0;
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
        {
            AvailableMillions = EnemyData.GetAvailableMillions(Floor);
            AvailableDemonGergons = EnemyData.GetNextDemogorgon(Floor, out NextDemogorgonFloor);
        }
        SpawnEnemyTimer = new MyTimer(EnemyFirstHalfInterval, SpanwEnemy, true, false);
        SpawnLootTimer = new MyTimer(PotionInterval, SpawnLoot, true, false);
        //Debug.Log("NextDemogorgonFloor=" + NextDemogorgonFloor);
        IsDemogorgonFloor = CheckDemogorgon(Floor);
        IsInit = true;
        Debug.Log("Init BattleManager");
    }
    void InitBattleSetting()
    {
        if (TestMode)
            return;
        PotionInterval = GameSettingData.PotionInterval;
        EnemyFirstHalfInterval = GameSettingData.EnemyFirstHalfInterval;
        EnemySecondHalfInterval = GameSettingData.EnemySecondHalfInterval;
        EnemyFirstHalfMinCount = GameSettingData.EnemyFirstHalfMinCount;
        EnemyFirstHalfMaxCount = GameSettingData.EnemyFirstHalfMaxCount;
        EnemySecondHalfMinCount = GameSettingData.EnemySecondHalfMinCount;
        EnemySecondHalfMaxCount = GameSettingData.EnemySecondHalfMaxCount;
        EnemySpawnInterval = GameSettingData.EnemySpawnInterval;
        MaxEnemy = GameSettingData.MaxEnemy;
        MaxLoot = GameSettingData.MaxLoot;
        FloorPlate = GameSettingData.FloorPlate;
        BossDebutPlate = GameSettingData.BossDebutPlate;
    }
    public static void AddEnemyKill()
    {
        EnemyKill++;
    }
    int GetRandomEnemySpawnfCount()
    {
        int spawnCount = 0;
        if (IsFirstHalf)
            spawnCount = Random.Range(EnemyFirstHalfMinCount, EnemyFirstHalfMaxCount);
        else
            spawnCount = Random.Range(EnemySecondHalfMinCount, EnemySecondHalfMaxCount);
        return spawnCount;
    }
    void SpawnDemogorgon()
    {
        for (int i = 0; i < AvailableDemonGergons.Count; i++)
        {
            EnemyRole er = Instantiate(AvailableDemonGergons[i], Vector3.zero, Quaternion.identity) as EnemyRole;
            er.SetEnemyData(GameDictionary.EnemyDic[AvailableDemonGergons[i].ID]);
            //Set SpawnPos
            er.transform.SetParent(EnemyParent);
            EnemyList.Add(er);
        }
        AvailableDemonGergons = EnemyData.GetNextDemogorgon(Floor + 1, out NextDemogorgonFloor);
        //Debug.Log("NextDemogorgonFloor=" + NextDemogorgonFloor);
        IsDemogorgonFloor = false;
    }
    void SpanwEnemy()
    {
        if (!CheckEnemySpawnLimit())
        {
            CurSpawnCount = 0;
            SpawnEnemyTimer.StartRunTimer = true;
            UpdateSpawnEnmeyTimer();
            return;
        }
        if (AvailableMillions.Count == 0)
            return;

        EnemyRole er;
        if (DesignatedEnemy && TestMode)
            er = Instantiate(DesignatedEnemy, Vector3.zero, Quaternion.identity) as EnemyRole;
        else
        {
            int rndEnemy = Random.Range(0, AvailableMillions.Count);
            er = Instantiate(AvailableMillions[rndEnemy], Vector3.zero, Quaternion.identity) as EnemyRole;
            if (GameDictionary.EnemyDic.ContainsKey(AvailableMillions[rndEnemy].ID))
                er.SetEnemyData(GameDictionary.EnemyDic[AvailableMillions[rndEnemy].ID]);
        }
        CurSpawnCount++;
        er.transform.SetParent(EnemyParent);
        AIRoleMove ai = er.GetComponent<AIRoleMove>();
        Vector2 offset = ai.SetRandDestination();
        er.transform.position = GetSpawnPos(offset);

        if (CurSpawnCount < GetRandomEnemySpawnfCount())
            StartCoroutine(WaitToSpawnEnemy());
        else
        {
            CurSpawnCount = 0;
            SpawnEnemyTimer.StartRunTimer = true;
            UpdateSpawnEnmeyTimer();
        }
        EnemyList.Add(er);
    }
    Vector2 GetSpawnPos(Vector2 _offset)
    {
        Vector2 pos1 = new Vector2(_offset.x, ScreenSize.y / 2);
        Vector2 pos2 = new Vector2(_offset.x, -ScreenSize.y / 2);
        Vector2 pos3 = new Vector2(ScreenSize.x / 2, _offset.y);
        Vector2 pos4 = new Vector2(-ScreenSize.x / 2, _offset.y);
        List<Vector2> posList = new List<Vector2>();
        posList.Add(pos1);
        posList.Add(pos2);
        posList.Add(pos3);
        posList.Add(pos4);
        Vector3 resultPos = Vector2.zero;
        float curDist = float.MaxValue;
        for(int i=0;i<posList.Count;i++)
        {
            if (Vector2.Distance(_offset, posList[i]) < curDist)
            {
                curDist = Vector2.Distance(_offset, posList[i]);
                resultPos = posList[i];
            }
        }
        resultPos += MyCameraControler.transform.position;
        return resultPos;
    }
    void UpdateSpawnEnmeyTimer()
    {
        //每次出怪後重新確認出怪時間
        if (IsFirstHalf)
            SpawnEnemyTimer.ResetMaxTime(EnemyFirstHalfInterval);
        else
            SpawnEnemyTimer.ResetMaxTime(EnemySecondHalfInterval);
    }
    bool CheckEnemySpawnLimit()
    {
        if (MaxEnemy == 0)
            return true;
        int cout = 0;
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i].isActiveAndEnabled)
                cout++;
        }
        if (cout < MaxEnemy)
            return true;
        return false;
    }
    bool CheckLootSpawnLimit()
    {
        if (MaxLoot == 0)
            return true;
        int cout = 0;
        for (int i = 0; i < LootList.Count; i++)
        {
            if (LootList[i].isActiveAndEnabled)
                cout++;
        }
        if (cout < MaxLoot)
            return true;
        return false;
    }
    void SpawnLoot()
    {
        if (!CheckLootSpawnLimit())
        {
            SpawnLootTimer.StartRunTimer = true;
            return;
        }
        Loot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as Loot;
        //Set SpawnPos
        loot.transform.SetParent(LootParetn);
        AIMove ai = loot.GetComponent<AIMove>();
        Vector2 offset = ai.SetRandDestination();
        loot.transform.position = GetSpawnPos(offset);
        SpawnLootTimer.StartRunTimer = true;
        LootList.Add(loot);
    }
    IEnumerator WaitToSpawnEnemy()
    {
        yield return new WaitForSeconds(EnemySpawnInterval);
        SpanwEnemy();
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
        if (IsInit)
        {
            InActivityOutSideEnemysAndLoots();
            UpdateCurPlate();
            if (SpawnEnemyTimer!=null)
                SpawnEnemyTimer.RunTimer();
            if (SpawnLootTimer!=null)
                SpawnLootTimer.RunTimer();
        }
        else if (Player.IsInit)
            Init();
    }
    void InActivityOutSideEnemysAndLoots()
    {
        DestructMargin_Left = (MyCameraControler.transform.position.x - (ScreenSize.x / 2 + 200));
        DestructMargin_Right = (MyCameraControler.transform.position.x + (ScreenSize.x / 2 + 200));
        //Enemys
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null)
                EnemyList.RemoveAt(i);
            else
            {
                if (EnemyList[i].Type != EnemyType.Demogorgon)
                {
                    if (EnemyList[i].transform.position.x < DestructMargin_Left ||
    EnemyList[i].transform.position.x > DestructMargin_Right)
                    {
                        EnemyList[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        EnemyList[i].gameObject.SetActive(true);
                    }
                }
            }
        }
        //Loots
        for (int i = 0; i < LootList.Count; i++)
        {
            if (LootList[i] == null)
                LootList.RemoveAt(i);
            else
            {
                if (LootList[i].transform.position.x < DestructMargin_Left ||
LootList[i].transform.position.x > DestructMargin_Right)
                {
                    LootList[i].gameObject.SetActive(false);
                }
                else
                    LootList[i].gameObject.SetActive(true);
            }
        }
    }
}
