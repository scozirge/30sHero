using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DungeonPlate : Plate
{
    int FloorPlateCount;
    [SerializeField]
    Image BottomImage;
    [SerializeField]
    List<Color> Colors;

    int CurFloor;
    static Dictionary<int, Color> FloorColor = new Dictionary<int, Color>();

    public override void Init(int _column, int _maxColumn)
    {
        base.Init(_column, _maxColumn);
        CurPlate = ColumnRank - 1 + 100000;
        FloorPlateCount = BattleManage.BM.FloorPlate;
        UpdatePlateColor();
    }
    public override void LevelDown()
    {
        base.LevelDown();
        UpdatePlateColor();
    }
    public override void LevelUp()
    {
        base.LevelUp();
        UpdatePlateColor();
    }
    void UpdatePlateColor()
    {
        if (BottomImage == null)
            return;
        if (Colors.Count == 0)
            return;
        CurFloor = CurPlate / FloorPlateCount - 9999;
        if (!FloorColor.ContainsKey(CurFloor))
        {
            FloorColor.Add(CurFloor, GetCurFloorColor(CurFloor));
        }

        BottomImage.color = FloorColor[CurFloor];
    }
    Color GetRandomColor(int _floor)
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < Colors.Count; i++)
        {
            colors.Add(Colors[i]);
        }
        if (FloorColor.ContainsKey(_floor - 1))
            colors.Remove(FloorColor[_floor - 1]);
        if (FloorColor.ContainsKey(_floor + 1))
            colors.Remove(FloorColor[_floor + 1]);
        int random = Random.Range(0, colors.Count);
        return colors[random];
    }
    Color GetCurFloorColor(int _floor)
    {
        int index = _floor % Colors.Count - 1;
        if (index < 0)
            index = Colors.Count - 1;
        else if (index > Colors.Count - 1)
            index = 0;
        return Colors[index];
    }
}
