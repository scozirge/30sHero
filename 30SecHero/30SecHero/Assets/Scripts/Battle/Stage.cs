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
    MyText MeterText;
    [SerializeField]
    Gate GatePrefab;
    [SerializeField]
    Gate EntrancePrefab;
    [SerializeField]
    Transform GateParent;


    static bool IsFirstHalf
    {
        get
        {
            float t = CurPlate % BM.FloorPlate;
            return t <= (BM.FloorPlate / 2);
        }
    }
    static int CurPlate = 0;
    void UpdateCurPlate()
    {
        if (!MyPlayer)
            return;
        CurPlate = (int)((BM.MyPlayer.transform.position.x + 1.5 * BM.PlateSizeX) / BM.PlateSizeX);
        BM.MeterText.text = string.Format("{0}{1}", CurPlate, StringData.GetString("Meter"));
        if (IsDemogorgonFloor)
        {
            if (CurPlate == NextDemogorgonFloor * FloorPlate - BossDebutPlate)
                SpawnDemogorgon();
        }
    }
    void InitStage()
    {
        if (!MyPlayer)
            return;
        Floor = (int)(CurPlate / BM.FloorPlate) + 1;
        UpdateFloorText();
        SpawnGate(Floor - 1);
        SpawnGate(Floor);
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
    }
    public static bool CheckDemogorgon(int _floor)
    {
        return (_floor == NextDemogorgonFloor);
    }
}
