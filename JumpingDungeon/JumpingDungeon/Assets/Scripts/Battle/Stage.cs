using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BattleManage
{
    [SerializeField]
    MyText FloorText;
    [SerializeField]
    MyText MeterText;
    [SerializeField]
    int MeterDistance;
    [SerializeField]
    int FloorMeter;
    [SerializeField]
    Gate GatePrefab;
    [SerializeField]
    Transform GateParent;

    float StartOffsetPos;
    static int StartMeter;
    static int CurMeter;
    void UpdateCurMeter()
    {
        if (!MyPlayer)
            return;  
        CurMeter = (int)((BM.MyPlayer.transform.position.x - StartOffsetPos) / BM.MeterDistance);
        BM.MeterText.text = string.Format("{0}{1}", CurMeter, StringData.GetString("Meter"));
    }
    void InitStage()
    {
        if (!MyPlayer)
            return; 
        StartOffsetPos = BM.MyPlayer.transform.position.x;
        StartMeter = (int)((MyPlayer.transform.position.x - StartOffsetPos) / MeterDistance);
        UpdateFloor();
        SpawnGate(Floor - 1);
        SpawnGate(Floor);
    }
    public static void SetFloor(int _floor)
    {
        Floor = _floor;
        BM.FloorText.text = string.Format("{0}{1}", Floor, StringData.GetString("Floor"));
    }
    static void UpdateFloor()
    {
        Floor = (int)((StartMeter - CurMeter) / BM.FloorMeter) + 1;
        BM.FloorText.text = string.Format("{0}{1}", Floor, StringData.GetString("Floor"));
    }
    static void SpawnGate(int _floor)
    {
        Gate gate = Instantiate(BM.GatePrefab, Vector3.zero, Quaternion.identity) as Gate;
        gate.transform.SetParent(BM.GateParent);
        gate.Init(_floor);
        if (_floor == 0)
            gate.transform.position = new Vector2(_floor * BM.FloorMeter * BM.MeterDistance - 850, 0);
        else
            gate.transform.position = new Vector2(_floor * BM.FloorMeter * BM.MeterDistance, 0);
    }
    public static void SpawnNextGate(int _destroyedFloor)
    {
        UpdateFloor();
        if (Floor > _destroyedFloor)
            SpawnGate(_destroyedFloor - 1);
        else
            SpawnGate(_destroyedFloor + 1);

    }
}
