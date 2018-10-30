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
    int MaxForeEnemy;
    [SerializeField]
    int MaxBackEnemy;
    [SerializeField]
    int MaxLoot;
    [SerializeField]
    List<EnemyRole> Enemys;
    [SerializeField]
    EnemyRole DesignatedEnemy;
    [SerializeField]
    PotionLoot LootPrefab;
    [SerializeField]
    CameraController CameraControler;
    [SerializeField]
    public PlayerRole MyPlayer;
    [SerializeField]
    GameObject SceneObject;
    [SerializeField]
    int EnemyDistance = 200;
    [SerializeField]
    int MaxRefindTimes = 20;
    [SerializeField]
    GameObject SetObj;
    [SerializeField]
    MyToggle MusicToggle;
    [SerializeField]
    MyToggle SoundToggle;

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
    static float DisableMargin_Left;
    static float DisableMargin_Right;
    static float DestructMargin_Left;
    static float DestructMargin_Right;
    public static float DisableMarginLengh = 300;
    public static float DestructMarginLength = 2000;
    List<EnemyRole> EnemyList = new List<EnemyRole>();
    List<PotionLoot> LootList = new List<PotionLoot>();
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
        PotionLoot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as PotionLoot;
        //Set SpawnPos
        loot.transform.SetParent(LootParetn);
        AIMove ai = loot.GetComponent<AIMove>();
        Vector2 offset = ai.SetRandDestination();
        loot.transform.position = GetSpawnPos(offset);
        SpawnLootTimer.StartRunTimer = true;
        LootList.Add(loot);
    }
    public void Set(bool _active)
    {
        SetObj.SetActive(_active);
        if (_active)
        {
            if (Player.MusicOn)
                MusicToggle.isOn = true;
            else
                MusicToggle.isOn = false;
            if (Player.SoundOn)
                SoundToggle.isOn = true;
            else
                SoundToggle.isOn = false;
        }
    }
    public void SetMusic()
    {
        Player.SetMusic(MusicToggle.isOn);
    }
    public void SetSound()
    {
        Player.SetSound(SoundToggle.isOn);
    }
    public static void RemoveEnemy(EnemyRole _er)
    {
        BM.EnemyList.Remove(_er);
    }
    public static void RemoveLoot(PotionLoot _loot)
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
            if (SpawnEnemyTimer != null)
                SpawnEnemyTimer.RunTimer();
            if (SpawnLootTimer != null)
                SpawnLootTimer.RunTimer();
        }
        else if (Player.IsInit)
            Init();
    }
    void InActivityOutSideEnemysAndLoots()
    {
        DisableMargin_Left = (MyCameraControler.transform.position.x - (ScreenSize.x / 2 + DisableMarginLengh));
        DisableMargin_Right = (MyCameraControler.transform.position.x + (ScreenSize.x / 2 + DisableMarginLengh));
        DestructMargin_Left = DisableMargin_Left - DestructMarginLength;
        DestructMargin_Right = DisableMargin_Right + DestructMarginLength;
        //Enemys
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null)
                EnemyList.RemoveAt(i);
            else
            {
                if (EnemyList[i].Type != EnemyType.Demogorgon)
                {
                    if (EnemyList[i].transform.position.x < DisableMargin_Left ||
                        EnemyList[i].transform.position.x > DisableMargin_Right)
                    {
                        EnemyList[i].gameObject.SetActive(false);
                        if (EnemyList[i].transform.position.x < DestructMargin_Left ||
                            EnemyList[i].transform.position.x > DestructMargin_Right)
                        {
                            EnemyList[i].SelfDestroy();
                        }
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
