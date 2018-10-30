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
    protected RectTransform LocationCriterion;
    [SerializeField]
    protected RectTransform Pivot;
    float LocationCriterionWidth;

    void InitStage()
    {
        if (!MyPlayer)
            return;
        LocationCriterionWidth = LocationCriterion.rect.width - 21;
        Floor = (int)(CurPlate / BM.FloorPlate) + 1;
        UpdateFloorText();
        SpawnGate(Floor - 1);
        SpawnGate(Floor);
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
        CurPlate = (int)((BM.MyPlayer.transform.position.x + 1.5 * BM.PlateSizeX) / BM.PlateSizeX);
        float lastDoorPos = (float)(((Floor-1) * BM.FloorPlate * BM.PlateSizeX) - (BM.PlateSizeX * 1.5f));
        FloorProcessingRatio = (float)(BattleManage.BM.MyPlayer.transform.position.x - lastDoorPos) / (BM.PlateSizeX * BM.FloorPlate);

        Pivot.localPosition = new Vector2(FloorProcessingRatio * LocationCriterionWidth, Pivot.localPosition.y);
        BM.VelocityText.text = string.Format("{0}{1}", (int)BM.MyPlayer.MoveSpeed, StringData.GetString("Meter"));
        if (IsDemogorgonFloor)
        {
            if (CurPlate == NextDemogorgonFloor * FloorPlate - BossDebutPlate)
                SpawnDemogorgon();
        }
    }

    static void UpdateFloorText()
    {
        BM.FloorText.text = string.Format("{0}{1}", Floor, StringData.GetString("Floor"));
    }
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
        gate.transform.position = new Vector2((_floor * BM.FloorPlate * BM.PlateSizeX) - (BM.PlateSizeX * 1.5f), 0);
    }
    public static void SpawnNextGate(int _destroyedFloor)
    {

        if (Floor > _destroyedFloor)
        {
            SpawnGate(_destroyedFloor - 1);
            Floor--;
            UpdateFloorText();
        }
        else
        {
            SpawnGate(_destroyedFloor + 1);
            Floor++;
            UpdateFloorText();
        }
        if (!BM.TestMode)
        {
            AvailableMillions = EnemyData.GetAvailableMillions(Floor);
            IsDemogorgonFloor = CheckDemogorgon(Floor);
        }
        PassFloorCount++;
        if (Floor > MaxFloor)
            MaxFloor = Floor;
    }
    public static bool CheckDemogorgon(int _floor)
    {
        return (_floor == NextDemogorgonFloor);
    }
}
