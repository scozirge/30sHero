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
    [SerializeField]
    List<GameObject> TutorialPages;
    [SerializeField]
    Text PageText;
    int CurTutorialPage;
    int MaxTutorialPage;
    [SerializeField]
    GameObject TPageArrow_Left;
    [SerializeField]
    GameObject TPageArrow_Right;
    [SerializeField]
    Animator DirectArrowAni;
    [SerializeField]
    GameObject PopupTutorialGo;
    [SerializeField]
    Text PopupTutorialTitle;
    [SerializeField]
    Text PopupTutorialDescription;
    [SerializeField]
    Image PopupTutorialPic_Left;
    [SerializeField]
    Image PopupTutorialPic_Right;
    [SerializeField]
    GameObject PopupTutorialGo_Left;
    [SerializeField]
    GameObject PopupTutorialGo_Right;
    [SerializeField]
    List<Sprite> PopupTutorialSprites;



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
        CurTutorialPage = 0;
        TutorialPages.RemoveAll(item => item == null);
        MaxTutorialPage = TutorialPages.Count;
        UpdateTutorialPage();
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
        MyPlayer.InitPlayerProperties();
        MyPlayer.UpdateAvatarTimeUI();
        //第一次進戰鬥跳教學
        if (Player.Tutorial)
        {
            Setting(true);
            OpenTutorial();
            IsPause = true;
            SoulGo.SetActive(false);
            MyCameraControler.enabled = false;
            gameObject.SetActive(false);
            //如果第一次進入教學，行進方向箭頭指示改在關掉教學介面時顯示
        }
        else//行進方向箭頭指示
            DirectArrowAni.SetTrigger("Play");
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
        if(ProbabilityGetter.GetResult(GameSettingData.PotionProportion))
        {
            PotionLoot loot = Instantiate(LootPrefab, Vector3.zero, Quaternion.identity) as PotionLoot;
            //Set SpawnPos
            loot.transform.SetParent(LootParetn);
            AIMove ai = loot.GetComponent<AIMove>();
            Vector2 offset = ai.SetRandDestination();
            loot.transform.position = GetSpawnPos(offset);
            LootList.Add(loot);
        }
        SpawnLootTimer.StartRunTimer = true;
    }

    public void Tutorial_NextPage(bool _next)
    {
        TutorialPages[CurTutorialPage].SetActive(false);
        if (_next)
        {
            CurTutorialPage++;
            if (CurTutorialPage >= MaxTutorialPage)
                CurTutorialPage = MaxTutorialPage - 1;
        }
        else
        {
            CurTutorialPage--;
            if (CurTutorialPage < 0)
                CurTutorialPage = 0;
        }
        TutorialPages[CurTutorialPage].SetActive(true);
        if (CurTutorialPage == 0)
            TPageArrow_Left.SetActive(false);
        else
            TPageArrow_Left.SetActive(true);

        if (CurTutorialPage == MaxTutorialPage - 1)
            TPageArrow_Right.SetActive(false);
        else
            TPageArrow_Right.SetActive(true);
        PageText.text = string.Format("{0}/{1}", CurTutorialPage + 1, MaxTutorialPage);
    }
    public void UpdateTutorialPage()
    {
        for (int i = 0; i < MaxTutorialPage; i++)
        {
            if (i != CurTutorialPage)
                TutorialPages[i].SetActive(false);
            else
                TutorialPages[i].SetActive(true);
        }
        if (CurTutorialPage == 0)
            TPageArrow_Left.SetActive(false);
        else
            TPageArrow_Left.SetActive(true);

        if (CurTutorialPage == MaxTutorialPage - 1)
            TPageArrow_Right.SetActive(false);
        else
            TPageArrow_Right.SetActive(true);
        PageText.text = string.Format("{0}/{1}", CurTutorialPage + 1, MaxTutorialPage);
    }
    public void PopupTutorial(string _type)
    {
        Pause(true);
        PopupTutorialGo.SetActive(true);
        PopupTutorialGo_Left.SetActive(true);
        PopupTutorialGo_Right.SetActive(true);
        switch (_type)
        {
            case "AvataEnergy":
                PopupTutorialDescription.text = StringData.GetString("EnergyPotionDescription");
                PopupTutorialTitle.text = StringData.GetString("EnergyPotionTitle");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[0];
                PopupTutorialPic_Right.sprite = PopupTutorialSprites[1];
                break;
            case "DamageUp":
                PopupTutorialDescription.text = StringData.GetString("DamagePotionDescription");
                PopupTutorialTitle.text = StringData.GetString("DamagePotionTitle");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[2];
                PopupTutorialPic_Right.sprite = PopupTutorialSprites[3];
                break;
            case "HPRecovery":
                PopupTutorialDescription.text = StringData.GetString("HealthPotionDescription");
                PopupTutorialTitle.text = StringData.GetString("HealthPotionTitle");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[4];
                PopupTutorialPic_Right.sprite = PopupTutorialSprites[5];
                break;
            case "Immortal":
                PopupTutorialDescription.text = StringData.GetString("ImmortalPotionDescription");
                PopupTutorialTitle.text = StringData.GetString("ImmortalPotionTitle");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[6];
                PopupTutorialPic_Right.sprite = PopupTutorialSprites[7];
                break;
            case "SpeedUp":
                PopupTutorialDescription.text = StringData.GetString("SpeedPotionDescription");
                PopupTutorialTitle.text = StringData.GetString("SpeedPotionTitle");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[8];
                PopupTutorialPic_Right.sprite = PopupTutorialSprites[9];
                break;
            case "Slime":
                PopupTutorialDescription.text = StringData.GetString("AvatarTimeDescription");
                PopupTutorialTitle.text = StringData.GetString("Slime");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[10];
                PopupTutorialPic_Right.sprite = PopupTutorialSprites[11];
                break;
            case "Ignite":
                PopupTutorialGo_Right.SetActive(false);
                PopupTutorialDescription.text = StringData.GetString("IgniteDescription");
                PopupTutorialTitle.text = StringData.GetString("Ignite");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[12];
                break;
            case "Freeze":
                PopupTutorialGo_Right.SetActive(false);
                PopupTutorialDescription.text = StringData.GetString("FreezeDescription");
                PopupTutorialTitle.text = StringData.GetString("Freeze");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[13];
                break;
            case "Poisoned":
                PopupTutorialGo_Right.SetActive(false);
                PopupTutorialDescription.text = StringData.GetString("PoisonedDescription");
                PopupTutorialTitle.text = StringData.GetString("Poisoned");
                PopupTutorialPic_Left.sprite = PopupTutorialSprites[14];
                break;
        }
    }
    public void OpenTutorial()
    {
        CurTutorialPage = 0;
        UpdateTutorialPage();
        TutorialGo.SetActive(true);
    }
    public void ClosePopupTutorial()
    {
        PopupTutorialGo.SetActive(false);
        Pause(false);
    }
    public void CloseTutorial()
    {
        if (Player.Tutorial)
        {
            if (CurTutorialPage < (MaxTutorialPage - 1))
            {
                Tutorial_NextPage(true);
            }
            else
            {
                DirectArrowAni.SetTrigger("Play");
                PlayerPrefs.SetInt(LocoData.Tutorial.ToString(), 1);
                Player.SetTutorial(false);
                Setting(false);
                Set(true);
                TutorialGo.SetActive(false);
            }
        }
        else
        {
            Setting(false);
            Set(true);
            TutorialGo.SetActive(false);
        }
    }
    public void Pause(bool _pause)
    {
        IsPause = _pause;
        SoulGo.SetActive(!_pause);
        MyCameraControler.enabled = !_pause;
        gameObject.SetActive(!_pause);
    }
    public void Setting(bool _active)
    {
        if (MyPlayer == null)
            return;
        Pause(_active);
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
                if (WaitToCallEnchantUI != null)
                    WaitToCallEnchantUI.RunTimer();
                if (WaitToCalculateResult != null)
                    WaitToCalculateResult.RunTimer();
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
        ReadyToGetEnchant = false;
    }
}
