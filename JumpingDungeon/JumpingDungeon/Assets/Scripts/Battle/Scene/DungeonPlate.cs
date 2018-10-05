using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DungeonPlate : Plate
{
    [SerializeField]
    int FloorPlateCount;
    [SerializeField]
    bool RandomColor;
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
        CurFloor = CurPlate / 4;
        if (!FloorColor.ContainsKey(CurFloor))
        {
            FloorColor.Add(CurFloor, GetRandomColor(CurFloor));
        }

        BottomImage.color = FloorColor[CurFloor];
    }
    Color GetRandomColor(int _floor)
    {
        List<Color> colors = Colors;
        if (FloorColor.ContainsKey(_floor - 1))
            colors.Remove(FloorColor[_floor - 1]);
        if (FloorColor.ContainsKey(_floor + 1))
            colors.Remove(FloorColor[_floor + 1]);
        int random = Random.Range(0, colors.Count);
        return colors[random];
    }
}
