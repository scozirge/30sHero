using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManage
{
    [SerializeField]
    public int StartFloorPlate;
    [SerializeField]
    int PlateSizeX;
    [SerializeField]
    int BossDebutPlate;
    [SerializeField]
    MyText FloorText;
    [SerializeField]
    MyText PlateText;
    [SerializeField]
    MyText VelocityText;
    [SerializeField]
    Gate GatePrefab;
    [SerializeField]
    Gate EntrancePrefab;
    [SerializeField]
    Transform GateParent;
    [SerializeField]
    StageSpawner MyStageSpawner;

    [SerializeField]
    protected RectTransform LocationCriterion;
    [SerializeField]
    protected RectTransform Pivot;
    [SerializeField]
    bool DontHideStage;
    [SerializeField]
    Vector2 TopFGIntervalMinMax;
    [SerializeField]
    Vector2 BotFGIntervalMinMax;

    float LocationCriterionWidth;
    static List<Stage> StageList;
    static List<ForeGround> FGList;
    static Dictionary<int, Vector2> GatePosDic;

    void InitStage()
    {
        if (!MyPlayer)
            return;
        Floor = GetCurFloor();
        MyStageSpawner.Init();
        LocationCriterionWidth = LocationCriterion.rect.width - 21;
        StageList = new List<Stage>();
        FGList = new List<ForeGround>();
        GatePosDic = new Dictionary<int, Vector2>();
        UpdateCurPlate();
        if (Floor <= 1)
            SpawnGate(Floor - 1);
        else//建立門(上一層的門不要生)
        {
            SpawnGate(Floor - 2);
            Vector2 pos = new Vector2(((Floor - StartFloor) * GetFloorPlateCount(Floor) * BM.PlateSizeX) - (BM.PlateSizeX * 1.5f), 0);
            GatePosDic.Add(Floor - 1, pos);
        }

        SpawnGate(Floor);
        //建立地形
        SpawnStage(new Vector2(BM.PlateSizeX, 0), GetFloorPlateCount(Floor) - 2, Floor);//目前層的地形
        SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f), 0), (BM.PlateSizeX * GetFloorPlateCount(Floor)), Floor);//目前層的前景
        SpawnStage(new Vector2((BM.PlateSizeX * GetFloorPlateCount(Floor + 1)) - BM.PlateSizeX, 0), GetFloorPlateCount(Floor + 1), Floor + 1);//下一層地形
        SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f) + (BM.PlateSizeX * GetFloorPlateCount(Floor + 1)), 0), (BM.PlateSizeX * GetFloorPlateCount(Floor + 1)), Floor + 1);//下一層的前景
        if ((Floor - 1) > 0)
        {
            SpawnStage(new Vector2(-((BM.PlateSizeX * GetFloorPlateCount(Floor - 1)) + (BM.PlateSizeX)), 0), GetFloorPlateCount(Floor - 1), Floor - 1);//上一層地形
            SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f) - (BM.PlateSizeX * GetFloorPlateCount(Floor - 1)), 0), (BM.PlateSizeX * GetFloorPlateCount(Floor - 1)), Floor - 1);//上一層的前景
        }
        if ((Floor - 2) > 0)//因為撞門才會生地形，但上一層的門不會生，所以要事先生地形
        {
            SpawnStage(new Vector2(-((BM.PlateSizeX * (GetFloorPlateCount(Floor - 1) + GetFloorPlateCount(Floor - 2))) + (BM.PlateSizeX)), 0), GetFloorPlateCount(Floor - 2), Floor - 2);//上上一層地形
            SpawnFG(new Vector2(-((BM.PlateSizeX * (GetFloorPlateCount(Floor - 1) + GetFloorPlateCount(Floor - 2))) + (BM.PlateSizeX)), 0), (BM.PlateSizeX * GetFloorPlateCount(Floor - 2)), Floor - 2);//上上一層的前景
        }
    }
    public static int GetFloorPlateCount(int _floor)
    {
        int plateCount = BM.StartFloorPlate + (_floor - 1) * GameSettingData.FloorUpPlate;
        if (plateCount < BM.StartFloorPlate)
            plateCount = BM.StartFloorPlate;
        return plateCount;
    }
    static bool IsFirstHalf
    {
        get
        {
            float t = CurPlate % GetFloorPlateCount(Floor);
            return t <= (GetFloorPlateCount(Floor) / 2);
        }
    }
    static int CurPlate = 0;
    static float FloorProcessingRatio = 0;
    float DistToPreviousDoor()
    {
        if (GatePosDic.ContainsKey(Floor - 1))
            return BattleManage.BM.MyPlayer.transform.position.x - GatePosDic[Floor - 1].x;
        return 0;
    }
    int GetCurFloor()
    {
        float distToFirstDoor = BattleManage.BM.MyPlayer.transform.position.x + (BM.PlateSizeX * 1.5f);
        float moveDist = Mathf.Abs(distToFirstDoor);
        int passFloor = 0;
        while (moveDist > 0)
        {
            if (distToFirstDoor >= 0)
                moveDist -= GetFloorPlateCount(StartFloor + passFloor) * BM.PlateSizeX;
            else
                moveDist -= GetFloorPlateCount(StartFloor - 1 - passFloor) * BM.PlateSizeX;
            if (moveDist > 0)
                passFloor++;
            else
                break;
        }
        if (distToFirstDoor > 0)
            return StartFloor + passFloor;
        else
            return StartFloor - passFloor - 1;
    }
    static Vector2 GetGatePosition(int _floor)
    {
        float posX = 0;
        int passFloor = Mathf.Abs(_floor - StartFloor);
        if (_floor >= StartFloor)
            passFloor += 1;
        else
            passFloor -= 1;
        for (int i = 0; i < passFloor; i++)
        {
            if (_floor < StartFloor)
            {
                posX -= (GetFloorPlateCount(StartFloor + ((i + 1) * -1)) * BM.PlateSizeX);
            }
            else
            {
                posX += (GetFloorPlateCount(StartFloor + i) * BM.PlateSizeX);
            }
        }
        posX -= BM.PlateSizeX * 1.5f;
        return new Vector2(posX, 0);
    }
    void UpdateCurPlate()
    {
        if (!MyPlayer)
            return;
        float distToFirstDoor = BattleManage.BM.MyPlayer.transform.position.x + (BM.PlateSizeX * 1.5f);
        Floor = GetCurFloor();//(int)(distToFirstDoor / (BM.PlateSizeX * GetFloorPlateCount(Floor))) + StartFloor;
        float distToPreviousDoor = DistToPreviousDoor();
        CurPlate = (int)(distToPreviousDoor / BM.PlateSizeX) % GetFloorPlateCount(Floor) + 1;
        FloorProcessingRatio = distToPreviousDoor / (BM.PlateSizeX * GetFloorPlateCount(Floor));
        PlateText.text = CurPlate.ToString();
        Pivot.localPosition = new Vector2(FloorProcessingRatio * LocationCriterionWidth, Pivot.localPosition.y);
        BM.VelocityText.text = string.Format("{0}{1}", (int)BM.MyPlayer.MoveSpeed, StringData.GetString("Meter"));
        if (IsDemogorgonFloor == 1 || IsDemogorgonFloor == 2)
        {
            if (CurPlate >= GetFloorPlateCount(Floor) - BossDebutPlate)
                SpawnDemogorgon(IsDemogorgonFloor);
        }
        //樓層改變
        if (Floor != LastFloor)
        {
            //更新怪物設定
            if (!BM.TestMode)
            {
                AvailableMillions = EnemyData.GetAvailableMillions(Floor);
                IsDemogorgonFloor = CheckDemogorgon(Floor);
            }
            //更新介面
            BM.FloorText.text = string.Format("{0}{1}", Floor, StringData.GetString("Floor"));
            //把距離太遠的地形隱藏
            BM.InActiveOutSideStage();
            LastFloor = Floor;
        }
    }
    int LastFloor = 0;
    static void SpawnGate(int _floor)
    {
        Gate gate;
        if (_floor != 0)
        {
            gate = Instantiate(BM.GatePrefab, Vector3.zero, Quaternion.identity) as Gate;
        }
        else
        {
            gate = Instantiate(BM.EntrancePrefab, Vector3.zero, Quaternion.identity) as Gate;
        }
        gate.transform.SetParent(BM.GateParent);
        gate.Init(_floor);
        gate.transform.position = GetGatePosition(_floor);//new Vector2(((_floor + 1 - StartFloor) * GetFloorPlateCount(_floor) * BM.PlateSizeX) - (BM.PlateSizeX * 1.5f), 0);
        if (!GatePosDic.ContainsKey(_floor))
            GatePosDic.Add(_floor, gate.transform.position);
    }
    public static void SpawnNextGate(int _destroyedFloor)
    {
        //史萊姆狀態衝撞城門有機會獲得額外金幣
        if (!BM.MyPlayer.IsAvatar && ProbabilityGetter.GetResult(BM.MyPlayer.MyEnchant[EnchantProperty.BreakDoorGold]))
        {
            ExtraDropGoldAdd(GameSettingData.FloorPassGold * Floor);
        }
        //史萊姆衝撞城門有機會變回英雄
        if (!BM.MyPlayer.IsAvatar && ProbabilityGetter.GetResult(BM.MyPlayer.MyEnchant[EnchantProperty.ReAvatar]))
        {
            BM.MyPlayer.ReAvatar();
        }
        //英雄衝撞城門獲得變身時間
        if (BM.MyPlayer.IsAvatar)
        {
            BM.MyPlayer.AddAvarTime(BM.MyPlayer.MyEnchant[EnchantProperty.Triumph]);
        }

        TransferToGainEquipDataList();//將目前吃到的裝備加到獲得裝備清單中
        if (Floor > _destroyedFloor)//上一層
        {
            SpawnGate(_destroyedFloor - 1);
            //建立地形
            SpawnStage(new Vector2(-(BM.PlateSizeX * GetFloorPlateCount(Floor - 2) * (StartFloor - Floor + 2) + BM.PlateSizeX), 0), GetFloorPlateCount(Floor - 2), Floor - 2);
            SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f + (BM.PlateSizeX * GetFloorPlateCount(Floor - 2) * (StartFloor - Floor + 2))), 0), (BM.PlateSizeX * GetFloorPlateCount(Floor - 2)), Floor - 2);
        }
        else//下一層
        {
            SpawnGate(_destroyedFloor + 1);
            //建立地形
            SpawnStage(new Vector2((BM.PlateSizeX * GetFloorPlateCount(Floor + 2)) * (Floor - StartFloor + 2) - BM.PlateSizeX, 0), GetFloorPlateCount(Floor + 2), Floor + 2);
            SpawnFG(new Vector2((-BM.PlateSizeX * 1.5f + (BM.PlateSizeX * GetFloorPlateCount(Floor + 2) * (Floor - StartFloor + 2))), 0), (BM.PlateSizeX * GetFloorPlateCount(Floor + 2)), Floor + 2);
        }
        PassFloorCount++;
        if (Floor > MaxFloor)
            MaxFloor = Floor;
    }
    public static int CheckDemogorgon(int _floor)
    {
        if ((_floor == NextDemogorgonFloor))
            return 1;
        else if ((_floor == PreviousDemogorgonFloor))
            return 2;
        else
            return 0;
    }
    static void SpawnStage(Vector2 _startPos, int _remainPlateSize, int _floor)
    {
        //Debug.Log(string.Format("Floor={0} StartPos={1} PlateSize={2}", _floor, _startPos, _remainPlateSize));
        List<Stage> stageList = StageSpawner.SpawnStage(_startPos, BM.PlateSizeX, _remainPlateSize, _floor);
        if (stageList == null)
            return;
        for (int i = 0; i < stageList.Count; i++)
        {
            StageList.Add(stageList[i]);
        }
    }
    static void SpawnFG(Vector2 _startPos, float _distance, int _floor)
    {
        if (BM.TopFGIntervalMinMax != Vector2.zero)
        {
            List<ForeGround> list = StageSpawner.SpawnFG(_startPos, _distance, (int)BM.TopFGIntervalMinMax.x, (int)BM.TopFGIntervalMinMax.y, _floor, true);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    FGList.Add(list[i]);
                }
            }
        }
        if (BM.BotFGIntervalMinMax != Vector2.zero)
        {
            List<ForeGround> list = StageSpawner.SpawnFG(_startPos, _distance, (int)BM.BotFGIntervalMinMax.x, (int)BM.BotFGIntervalMinMax.y, _floor, false);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    FGList.Add(list[i]);
                }
            }
        }
    }
    void InActiveOutSideStage()
    {
        if (DontHideStage)
            return;
        if (StageList == null)
            return;
        //地形
        for (int i = 0; i < StageList.Count; i++)
        {
            if (Mathf.Abs(StageList[i].Floor - Floor) > 1)
                StageList[i].gameObject.SetActive(false);
            else
                StageList[i].gameObject.SetActive(true);
        }
        //前景
        for (int i = 0; i < FGList.Count; i++)
        {
            if (Mathf.Abs(FGList[i].Floor - Floor) > 1)
                FGList[i].gameObject.SetActive(false);
            else
                FGList[i].gameObject.SetActive(true);
        }
    }
}
