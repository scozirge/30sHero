using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    GameObject SettingObj;
    [SerializeField]
    GameObject SetObj;
    [SerializeField]
    MyToggle MusicToggle;
    [SerializeField]
    MyToggle SoundToggle;
    [SerializeField]
    GameObject GiveUpConfirmObj;
    [SerializeField]
    GameObject BattleBG;
    [SerializeField]
    GameObject ExampleBattleBG;
    [SerializeField]
    GameObject SoulGo;
    [SerializeField]
    GameObject GetEnchantObj;
    [SerializeField]
    MyText GetEnchant_Name;
    [SerializeField]
    MyText GetEnchant_Description;
    [SerializeField]
    Image GetEnchant_Icon;
    [SerializeField]
    GameObject TutorialGo;
    [SerializeField]
    Animator WarningAni;

    static List<EnemyRole> AvailableMillions;
    static EnemyRole PreviousDemonGergons;
    static EnemyRole NextDemonGergons;
    int CurSpawnCount;
    Transform EnemyParent;
    Transform LootParetn;
    MyTimer SpawnEnemyTimer;
    MyTimer SpawnLootTimer;
    public static BattleManage BM;
    public static CameraController MyCameraControler;
    public static int StartFloor;
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
    static int PreviousDemogorgonFloor;
    static int IsDemogorgonFloor;
    bool IsInit;
    int EnemySpawnCount;

    static bool IsPause;


    // Use this for initialization
    void Awake()
    {
        if (!GameManager.IsInit)
            GameManager.DeployGameManager();
        SceneObject.SetActive(false);
        BattleBG.SetActive(false);
    }
    void Init()
    {
        ExampleBattleBG.SetActive(false);
        IsPause = false;
        SceneObject.SetActive(true);
        StartFloor = 1;
        InitSettlement();
        InitBattleSetting();
        BM = this;
        EnemyParent = GameObject.FindGameObjectWithTag("EnemyParent").GetComponent<Transform>();
        LootParetn = GameObject.FindGameObjectWithTag("LootParent").GetComponent<Transform>();
        CurSpawnCount = 0;
        EnemyKill = 0;
        InitStage();
        MyCameraControler = CameraControler;
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
            PreviousDemonGergons = EnemyData.GetPreviousDemogorgon(Floor, out PreviousDemogorgonFloor);
            NextDemonGergons = EnemyData.GetNextDemogorgon(Floor, out NextDemogorgonFloor);
            /*
            Debug.Log("PreviousDemonGergons=" + PreviousDemonGergons.ID);
            Debug.Log("NextDemonGergons=" + NextDemonGergons.ID);
            Debug.Log("PreviousDemogorgonFloor=" + PreviousDemogorgonFloor);
            Debug.Log("NextDemogorgonFloor=" + NextDemogorgonFloor);
            */
        }
        SpawnEnemyTimer = new MyTimer(EnemyFirstHalfInterval, SpanwEnemy, true, false);
        SpawnLootTimer = new MyTimer(PotionInterval, SpawnLoot, true, false);
        //Debug.Log("NextDemogorgonFloor=" + NextDemogorgonFloor);
        IsDemogorgonFloor = CheckDemogorgon(Floor);
        IsInit = true;
        BattleBG.SetActive(true);
        //第一次進戰鬥跳教學
        if (Player.Tutorial)
        {
            Setting(true);
            Tutorial(true);
            IsPause = true;
            SoulGo.SetActive(false);
            MyCameraControler.enabled = false;
            gameObject.SetActive(false);
            PlayerPrefs.SetInt(LocoData.Tutorial.ToString(), 1);
        }
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
        BossDebutPlate = GameSettingData.BossDebutPlate;
        StartFloor = Player.CurFloor;
        StartFloorPlate = GameSettingData.FloorPlate;
        if (StartFloor < 1)
            StartFloor = 1;
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
    public void Tutorial(bool _bool)
    {
        TutorialGo.SetActive(_bool);
    }
    public void Setting(bool _active)
    {
        if (MyPlayer == null)
            return;
        IsPause = _active;
        SoulGo.SetActive(!_active);
        MyCameraControler.enabled = !_active;
        gameObject.SetActive(!_active);
        SettingObj.SetActive(_active);
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
    public void GiveUpConfirm(bool _active)
    {
        GiveUpConfirmObj.SetActive(_active);
    }
    public void SetMusic()
    {
        Player.SetMusic(MusicToggle.isOn);
    }
    public void SetSound()
    {
        Player.SetSound(SoundToggle.isOn);
    }
    public static void AddEnemy(EnemyRole _er)
    {
        BM.EnemyList.Add(_er);
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
            if (!IsPause)
            {
                InActivityOutSideEnemysAndLoots();
                UpdateCurPlate();
                if (SpawnEnemyTimer != null)
                    SpawnEnemyTimer.RunTimer();
                if (SpawnLootTimer != null)
                    SpawnLootTimer.RunTimer();
            }
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
                        /*
                        if (EnemyList[i].transform.position.x < DestructMargin_Left ||
                            EnemyList[i].transform.position.x > DestructMargin_Right)
                        {
                            EnemyList[i].SelfDestroy();
                        }
                        */
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
    public void CallGetEnchantUI(bool _active)
    {
        IsPause = _active;
        SoulGo.SetActive(!_active);
        MyCameraControler.enabled = !_active;
        gameObject.SetActive(!_active);
        GetEnchantObj.SetActive(_active);
    }
}
