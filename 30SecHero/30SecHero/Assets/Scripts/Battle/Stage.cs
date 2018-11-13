using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManage
{
    [SerializeField]
    public int FloorPlate;
    [SerializeField]
    int PlateSizeX;
    [SerializeField]
    int BossDebutPlate;
    [SerializeField]
    MyText FloorText;
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

    void InitStage()
    {
        if (!MyPlayer)
            return;
        MyStageSpawner.Init();
        LocationCriterionWidth = LocationCriterion.rect.width - 21;
        StageList = new List<Stage>();
        FGList = new List<ForeGround>();
        UpdateCurPlate();
        //建立門
        SpawnGate(Floor - 1);
        SpawnGate(Floor);
        //建立地形
        SpawnStage(new Vector2(BM.PlateSizeX, 0), BM.FloorPlate - 2, Floor);//目前層的地形
        SpawnStage(new Vector2((BM.PlateSizeX * BM.FloorPlate) - BM.PlateSizeX, 0), BM.FloorPlate, Floor + 1);//下一層地形
        SpawnStage(new Vector2(-((BM.PlateSizeX * BM.FloorPlate) + (BM.PlateSizeX)), 0), BM.FloorPlate, Floor - 1);//上一層地形
        SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f), 0), (BM.PlateSizeX * BM.FloorPlate), Floor);//目前層的前景
        SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f) + (BM.PlateSizeX * BM.FloorPlate), 0), (BM.PlateSizeX * BM.FloorPlate), Floor + 1);//下一層的前景
        SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f) - (BM.PlateSizeX * BM.FloorPlate), 0), (BM.PlateSizeX * BM.FloorPlate), Floor - 1);//上一層的前景
    }

    static bool IsFirstHalf
    {
        get
        {
            float t = CurPlate % BM.FloorPlate;
            return t <= (BM.FloorPlate / 2);
        }
    }
    static int CurPlate = 0;
    static float FloorProcessingRatio = 0;
    void UpdateCurPlate()
    {
        if (!MyPlayer)
            return;

        //CurPlate = CurPlate % BM.FloorPlate;

        float distToFirstDoor = BattleManage.BM.MyPlayer.transform.position.x + (BM.PlateSizeX * 1.5f);
        Floor = (int)(distToFirstDoor / (BM.PlateSizeX * BM.FloorPlate)) + StartFloor;
        if (distToFirstDoor >= 0)
        {
            CurPlate = (int)(distToFirstDoor / BM.PlateSizeX) % (BM.FloorPlate) + 1;
            FloorProcessingRatio = (distToFirstDoor % (BM.PlateSizeX * BM.FloorPlate)) / (BM.PlateSizeX * BM.FloorPlate);
        }
        else
        {
            Floor -= 1;
            CurPlate = ((int)(distToFirstDoor / BM.PlateSizeX) + BM.FloorPlate) % (BM.FloorPlate);
            if (CurPlate == 0)
                CurPlate = 10;
            FloorProcessingRatio = (distToFirstDoor % (BM.PlateSizeX * BM.FloorPlate) + (BM.PlateSizeX * BM.FloorPlate)) / (BM.PlateSizeX * BM.FloorPlate);
        }
        Pivot.localPosition = new Vector2(FloorProcessingRatio * LocationCriterionWidth, Pivot.localPosition.y);
        BM.VelocityText.text = string.Format("{0}{1}", (int)BM.MyPlayer.MoveSpeed, StringData.GetString("Meter"));
        if (IsDemogorgonFloor)
        {
            if (CurPlate == NextDemogorgonFloor * FloorPlate - BossDebutPlate)
                SpawnDemogorgon();
        }
        if (Floor != LastFloor)
        {
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
        gate.transform.position = new Vector2(((_floor + 1 - StartFloor) * BM.FloorPlate * BM.PlateSizeX) - (BM.PlateSizeX * 1.5f), 0);
    }
    public static void SpawnNextGate(int _destroyedFloor)
    {
        TransferToGainEquipDataList();//將目前吃到的裝備加到獲得裝備清單中
        if (Floor > _destroyedFloor)//上一層
        {
            SpawnGate(_destroyedFloor - 1);
            if (!BM.TestMode)
            {
                AvailableMillions = EnemyData.GetAvailableMillions(Floor - 1);
                IsDemogorgonFloor = CheckDemogorgon(Floor - 1);
            }
            //建立地形
            SpawnStage(new Vector2(-(BM.PlateSizeX * BM.FloorPlate * (StartFloor - Floor + 2) + BM.PlateSizeX), 0), BM.FloorPlate, Floor - 2);
            SpawnFG(new Vector2(-(BM.PlateSizeX * 1.5f + (BM.PlateSizeX * BM.FloorPlate * (StartFloor - Floor + 2))), 0), (BM.PlateSizeX * BM.FloorPlate), Floor - 2);
        }
        else//下一層
        {
            SpawnGate(_destroyedFloor + 1);
            if (!BM.TestMode)
            {
                AvailableMillions = EnemyData.GetAvailableMillions(Floor + 1);
                IsDemogorgonFloor = CheckDemogorgon(Floor + 1);
            }
            //建立地形
            SpawnStage(new Vector2((BM.PlateSizeX * BM.FloorPlate) * (Floor - StartFloor + 2) - BM.PlateSizeX, 0), BM.FloorPlate, Floor + 2);
            SpawnFG(new Vector2((-BM.PlateSizeX * 1.5f + (BM.PlateSizeX * BM.FloorPlate * (Floor - StartFloor + 2))), 0), (BM.PlateSizeX * BM.FloorPlate), Floor + 2);
        }
        PassFloorCount++;
        if (Floor > MaxFloor)
            MaxFloor = Floor;
    }
    public static bool CheckDemogorgon(int _floor)
    {
        return (_floor == NextDemogorgonFloor);
    }
    static void SpawnStage(Vector2 _startPos, int _remainPlateSize, int _floor)
    {
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
