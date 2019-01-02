using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

partial class BattleManage
{
    [SerializeField]
    GameObject SettlementObj;
    [SerializeField]
    Text FloorClearText;
    [SerializeField]
    Text MaxFloorText;
    [SerializeField]
    Text MonsterKillsText;
    [SerializeField]
    Text BossKillsText;
    [SerializeField]
    Text UnlockPartnerText;
    [SerializeField]
    Text GoldText;
    [SerializeField]
    Text EmeraldText;
    [SerializeField]
    ItemSpawner MySpanwer;
    [SerializeField]
    float CallSettlementTime;
    [SerializeField]
    RunAnimatedText MyRunText;



    //結算資料
    static int NewFloorGolds;
    static int EnemyKillGolds;
    static int EnemyDropGolds;
    static int ExtraDropGolds;
    static float GoldsMultiple;
    static int BossDropEmeralds;
    static int PassFloorCount;
    static int PassNewFloorCount;
    static int MaxFloor;
    public static int EnemyKill;
    static int BossKill;
    static int UnlockPartner;

    static int TotalGold;
    static int TotalEmerald;
    public static List<EquipData> ExpectEquipDataList;
    static List<EquipData> GainEquipDataList;

    static void InitSettlement()
    {
        NewFloorGolds = 0;
        EnemyKillGolds = 0;
        EnemyDropGolds = 0;
        ExtraDropGolds = 0;
        GoldsMultiple = 0;
        BossDropEmeralds = 0;
        PassFloorCount = 0;
        MaxFloor = 0;


        TotalGold = 0;
        TotalEmerald = 0;
        GainEquipDataList = new List<EquipData>();
        ExpectEquipDataList = new List<EquipData>();
    }

    public static void EnemyDropGoldAdd(int _gold)
    {
        AudioPlayer.PlaySound(GameManager.GM.CoinSound);
        EnemyDropGolds += _gold;
        //Debug.Log("_gold=" + _gold);
        //Debug.Log("EnemyKillGolds=" + EnemyKillGolds);
    }
    public static void ExtraDropGoldAdd(int _gold)
    {
        AudioPlayer.PlaySound(GameManager.GM.CoinSound);
        ExtraDropGolds += _gold;
    }
    public static void BossDropEmeraldAdd(int _emerald)
    {
        AudioPlayer.PlaySound(GameManager.GM.CoinSound);
        BossDropEmeralds += _emerald;
        //Debug.Log("_emerald=" + _emerald);
        //Debug.Log("BossDropEmeralds=" + BossDropEmeralds);
    }
    public static void GainEquip(EquipData _data)
    {
        ExpectEquipDataList.Add(_data);
        //Debug.Log("UID=" + _data.UID);
        //Debug.Log("LV=" + _data.LV);
        //Debug.Log("Quaility=" + _data.Quality);
    }
    public static void TransferToGainEquipDataList()
    {
        for (int i = 0; i < ExpectEquipDataList.Count; i++)
        {
            GainEquipDataList.Add(ExpectEquipDataList[i]);
        }
        ExpectEquipDataList = new List<EquipData>();
    }
    public void CalculateResult()
    {
        //獎勵計算
        if (MaxFloor > Player.MaxFloor)
        {
            PassNewFloorCount = MaxFloor - Player.MaxFloor;
        }
        //尋寶專家(根據撞碰的城門增加獲得金幣百分比)
        GoldsMultiple = PassFloorCount * MyPlayer.MyEnchant[EnchantProperty.TreasureHunting];
        MaxFloor = (MaxFloor > Player.MaxFloor) ? MaxFloor : Player.MaxFloor;
        NewFloorGolds = (PassFloorCount * GameSettingData.FloorPassGold * Floor) + (PassNewFloorCount * GameSettingData.NewFloorPassGold * Floor);
        EnemyKillGolds = EnemyKill * GameSettingData.EnemyGold;
        TotalGold = (int)((NewFloorGolds + EnemyKillGolds + EnemyDropGolds + ExtraDropGolds) * (1 + GoldsMultiple));
        TotalEmerald = BossDropEmeralds;
        //寫入資料
        if (Player.LocalData)
        {
            //敵人擊殺
            if (EnemyKill > Player.MaxEnemyKills)
                Player.SetMaxEnemyKills_Local(EnemyKill);
            //目前樓層
            Player.SetCurFloor_Local(Floor);
            //最高樓層
            Player.SetMaxFloor_Local(MaxFloor);
            //金幣獲得
            Player.GainGold(TotalGold);
            //寶石獲得
            Player.GainEmerald(TotalEmerald);
            //裝備獲得
            Player.GainEquip_Local(GainEquipDataList);
            //顯示結果
            StartCoroutine(WaitToShowResult());
        }
        else
        {
            //送server處理
            Player.Settlement(Player.Gold + TotalGold, Player.Emerald + TotalEmerald, Floor, (MaxFloor > Player.MaxFloor) ? MaxFloor : Player.MaxFloor, GainEquipDataList);
        }
    }
    public IEnumerator WaitToShowResult()
    {
        yield return new WaitForSeconds(CallSettlementTime);
        ShowResult();
    }
    void ShowResult()
    {
        AudioPlayer.StopAllMusic();
        AudioPlayer.PlayMusicByAudioClip_Static(GameManager.GM.SettlementMusic);
        //顯示介面
        SettlementObj.SetActive(true);
        IsPause = true;
        SoulGo.SetActive(false);
        MyCameraControler.enabled = false;
        SceneObject.SetActive(false);
        //顯示資料
        SpawnEquipItem();
        FloorClearText.text = PassFloorCount.ToString();
        //MaxFloorText.text = Player.MaxFloor.ToString();
        MonsterKillsText.text = EnemyKill.ToString();
        BossKillsText.text = BossKill.ToString();
        UnlockPartnerText.text = UnlockPartner.ToString();
        GoldText.text = TotalGold.ToString();
        EmeraldText.text = TotalEmerald.ToString();
        //設定動畫文字
        MyRunText.Clear();
        MyRunText.SetAnimatedText("FloorClear", 0, PassFloorCount, FloorClearText);
        MyRunText.SetAnimatedText("MonsterKill", 0, EnemyKill, MonsterKillsText);
        MyRunText.SetAnimatedText("BossKill", 0, BossKill, BossKillsText);
        MyRunText.SetAnimatedText("UnlockPartner", 0, UnlockPartner, UnlockPartnerText);
        MyRunText.SetAnimatedText("Gold", 0, TotalGold, GoldText);
        MyRunText.SetAnimatedText("Emerald", 0, TotalEmerald, EmeraldText);

    }
    void SpawnEquipItem()
    {
        for (int i = 0; i < GainEquipDataList.Count; i++)
        {
            EquipItem ei = (EquipItem)MySpanwer.Spawn();
            ei.Set(GainEquipDataList[i], null);
        }
    }
    public void ReTry()
    {
        PopupUI.CallCutScene("Battle");
        //ChangeScene.GoToScene(MyScene.Battle);
    }
    public void BackToMenu()
    {
        PopupUI.CallCutScene("Main");
        //ChangeScene.GoToScene(MyScene.Main);
    }
    public static void AddEnemyKill()
    {
        EnemyKill++;
    }
    public static void AddBossKill()
    {
        BossKill++;
        //播放BOSS擊殺獎勵
        AudioPlayer.FadeOutMusic("BossFight", 0.5f);
        AudioPlayer.FadeInMusic(GameManager.GM.FightMusic1, "Battle", 2f);
        //不屈勇者(擊殺BOSS獲得額外變身秒數)
        BM.MyPlayer.AvatarTimer += BM.MyPlayer.MyEnchant[EnchantProperty.Courage];
        BattleManage.BM.MyPlayer.AvatarTimer += BattleManage.BM.MyPlayer.MyEnchant[EnchantProperty.Courage];
    }
    public static void AddUnlockPartner()
    {
        UnlockPartner++;
    }
    public static void KillNewBoss(int _id)
    {
        for (int i = 0; i < Player.KillBossID.Count; i++)
        {
            if (_id == Player.KillBossID[i])
            {
                return;
            }
        }
        //擊殺新BOSS
        Player.KillNewBoss(_id);
        GetEnchant();
    }
    public static void GetEnchant()
    {
        EnchantData ed = EnchantData.GetAvailableRandomEnchant();
        if (ed != null)
        {
            Player.EnchantUpgrade(ed);
            BM.GetEnchant_Name.text = ed.Name;
            BM.GetEnchant_Icon.sprite = ed.GetICON();
            BM.GetEnchant_Description.text = ed.Description(0);
            BM.CallGetEnchantUI(true);
        }
    }
}
