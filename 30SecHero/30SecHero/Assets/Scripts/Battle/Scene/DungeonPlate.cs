using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DungeonPlate : Plate
{
    int FloorPlateCount;
    [SerializeField]
    Image FloorImage;
    [SerializeField]
    Image WallTopImage;
    [SerializeField]
    Image WallTopLineImage;
    [SerializeField]
    Image WallTopShadowImage;
    [SerializeField]
    Image WallBotImage;
    [SerializeField]
    Image WallBotLineImage;
    [SerializeField]
    Image WallBotShadowImage;
    [SerializeField]
    Image FloorLineDraft;
    [SerializeField]
    Image FloorShadow;
    [SerializeField]
    List<Color> BotColors;
    [SerializeField]
    List<Color> ShadowColors;
    [SerializeField]
    List<Color> LineDraftColors;

    int CurFloor;
    static Dictionary<int, Color> FloorColor = new Dictionary<int, Color>();
    static Dictionary<int, Color> ShadowColor = new Dictionary<int, Color>();
    static Dictionary<int, Color> LineDraftColor = new Dictionary<int, Color>();

    public override void Init(int _column, int _maxColumn)
    {
        base.Init(_column, _maxColumn);
        CurPlate = ColumnRank;
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
        CurFloor = (CurPlate + FloorPlateCount-1) / FloorPlateCount - 1 + (BattleManage.StartFloor - 1);
        //底板
        if (!FloorColor.ContainsKey(CurFloor))
        {
            FloorColor.Add(CurFloor, GetNewCurFloorColor(CurFloor));
        }
        FloorImage.color = FloorColor[CurFloor];
        WallTopImage.color = FloorColor[CurFloor];
        WallBotImage.color = FloorColor[CurFloor];
        //光影
        if (!ShadowColor.ContainsKey(CurFloor))
        {
            ShadowColor.Add(CurFloor, GetCurShadowColor(CurFloor));
        }
        FloorShadow.color = ShadowColor[CurFloor];
        WallTopShadowImage.color = ShadowColor[CurFloor];
        WallBotShadowImage.color = ShadowColor[CurFloor];
        //線搞
        if (!LineDraftColor.ContainsKey(CurFloor))
        {
            LineDraftColor.Add(CurFloor, GetCurLineDraftColor(CurFloor));
        }
        FloorLineDraft.color = LineDraftColor[CurFloor];
        WallTopLineImage.color = LineDraftColor[CurFloor];
        WallBotLineImage.color = LineDraftColor[CurFloor];
    }
    Color GetRandomColor(int _floor)
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < BotColors.Count; i++)
        {
            colors.Add(BotColors[i]);
        }
        if (FloorColor.ContainsKey(_floor - 1))
            colors.Remove(FloorColor[_floor - 1]);
        if (FloorColor.ContainsKey(_floor + 1))
            colors.Remove(FloorColor[_floor + 1]);
        int random = Random.Range(0, colors.Count);
        return colors[random];
    }
    Color GetNewCurFloorColor(int _floor)
    {
        int index = _floor % BotColors.Count;
        if (index < 0)
            index = BotColors.Count + index;
        return BotColors[index];
    }
    Color GetCurFloorColor(int _floor)
    {
        int index = _floor % BotColors.Count;
        if (index < 0)
            index = BotColors.Count + index;
        return BotColors[index];
    }
    Color GetCurShadowColor(int _floor)
    {
        int index = _floor % ShadowColors.Count;
        if (index < 0)
            index = ShadowColors.Count + index;
        return ShadowColors[index];
    }
    Color GetCurLineDraftColor(int _floor)
    {
        int index = _floor % LineDraftColors.Count;
        if (index < 0)
            index = LineDraftColors.Count + index;
        return LineDraftColors[index];
    }
}
