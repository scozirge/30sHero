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
    Text GoldText;
    [SerializeField]
    Text EmeraldText;
    [SerializeField]
    ItemSpawner MySpanwer;




    //結算資料
    static int NewFloorGolds;
    static int EnemyKillGolds;
    static int EnemyDropGolds;
    static int BossDropEmeralds;
    static int PassFloorCount;
    static int PassNewFloorCount;
    static int MaxFloor;

    static int TotalGold;
    static int TotalEmerald;
    static List<EquipData> GainEquipDataList;

    static void InitSettlement()
    {
        NewFloorGolds = 0;
        EnemyKillGolds = 0;
        EnemyDropGolds = 0;
        BossDropEmeralds = 0;
        PassFloorCount = 0;
        MaxFloor = 0;

        TotalGold = 0;
        TotalEmerald = 0;
        GainEquipDataList = new List<EquipData>();
    }

    public static void EnemyDropGoldAdd(int _gold)
    {
        EnemyDropGolds += _gold;
        //Debug.Log("_gold=" + _gold);
        //Debug.Log("EnemyKillGolds=" + EnemyKillGolds);
    }
    public static void BossDropEmeraldAdd(int _emerald)
    {
        BossDropEmeralds += _emerald;
        //Debug.Log("_emerald=" + _emerald);
        //Debug.Log("BossDropEmeralds=" + BossDropEmeralds);
    }
    public static void GainEquip(EquipData _data)
    {
        GainEquipDataList.Add(_data);
        //Debug.Log("UID=" + _data.UID);
        //Debug.Log("LV=" + _data.LV);
        //Debug.Log("Quaility=" + _data.Quality);
    }
    public void ShowSettlement()
    {
        SettlementObj.SetActive(true);
        //資料設定
        //突破樓層
        if (MaxFloor > Player.MaxFloor)
        {
            PassNewFloorCount = MaxFloor - Player.MaxFloor;
            Player.SetMaxFloor(MaxFloor);
        }
        //敵人擊殺
        if (EnemyKill > Player.MaxEnemyKills)
            Player.SetMaxEnemyKills(EnemyKill);       
        //金幣獲得
        NewFloorGolds = (PassFloorCount * GameSettingData.FloorPassGold) + (PassNewFloorCount * GameSettingData.NewFloorPassGold);
        EnemyKillGolds = EnemyKill * GameSettingData.EnemyGold;
        TotalGold = NewFloorGolds + EnemyKillGolds + EnemyDropGolds;
        Player.GainGold(TotalGold);
        //寶石獲得
        TotalEmerald = BossDropEmeralds;
        Player.GainEmerald(TotalEmerald);
        //裝備獲得
        SpawnEquipItem();
        Player.GainEquip(GainEquipDataList);
        //顯示結算
        FloorClearText.text = Floor.ToString();
        MaxFloorText.text = Player.MaxFloor.ToString();
        MonsterKillsText.text = EnemyKill.ToString();
        GoldText.text = TotalGold.ToString();
        EmeraldText.text = TotalEmerald.ToString();
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
        ChangeScene.GoToScene(MyScene.Battle);
    }
    public void BackToMenu()
    {
        ChangeScene.GoToScene(MyScene.Main);
    }
}
