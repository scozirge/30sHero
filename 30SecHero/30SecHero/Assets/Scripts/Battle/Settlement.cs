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
    [SerializeField]
    float CallSettlementTime;



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
    public void CalculateResult()
    {
        //獎勵計算
        if (MaxFloor > Player.MaxFloor)
        {
            PassNewFloorCount = MaxFloor - Player.MaxFloor;
        }
        MaxFloor = (MaxFloor > Player.MaxFloor) ? MaxFloor : Player.MaxFloor;
        NewFloorGolds = (PassFloorCount * GameSettingData.FloorPassGold) + (PassNewFloorCount * GameSettingData.NewFloorPassGold);
        EnemyKillGolds = EnemyKill * GameSettingData.EnemyGold;
        TotalGold = NewFloorGolds + EnemyKillGolds + EnemyDropGolds;
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
        //顯示介面
        SettlementObj.SetActive(true);
        IsPause = true;
        SoulGo.SetActive(false);
        MyCameraControler.enabled = false;
        SceneObject.SetActive(false);
        //顯示資料
        SpawnEquipItem();
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
