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
    Color GetColorByCurPlate(int _plate, int _type)
    {
        _plate -= 1;
        int floor = BattleManage.StartFloor;
        int passPlate = Mathf.Abs(_plate);
        if (_plate < 0)
            floor--;
        while (passPlate > 0)
        {
            passPlate -= BattleManage.GetFloorPlateCount(floor);
            if (passPlate > 0)
                if (_plate >= 0)
                    floor++;
                else
                    floor--;
            else if(passPlate==0)
            {
                if (_plate >= 0)
                    floor++;
            }
        }
        Color color = GetCurFloorColor(floor - 1);
        switch (_type)
        {
            case 0:
                color = GetCurFloorColor(floor - 1);
                break;
            case 1:
                color = GetCurShadowColor(floor - 1);
                break;
            case 2:
                color = GetCurLineDraftColor(floor - 1);
                break;
            default:
                Debug.LogWarning("type wrong");
                break;
        }
        return color;
    }
    void UpdatePlateColor()
    {

        FloorPlateCount = BattleManage.GetFloorPlateCount(BattleManage.Floor);
        CurFloor = (CurPlate + FloorPlateCount - 1) / FloorPlateCount - 1 + (BattleManage.StartFloor - 1);
        //底板
        if (!FloorColor.ContainsKey(CurFloor))
        {
            FloorColor.Add(CurFloor, GetNewCurFloorColor(CurFloor));
        }
        FloorImage.color = GetColorByCurPlate(CurPlate, 0);
        WallTopImage.color = FloorImage.color;
        WallBotImage.color = FloorImage.color;
        /*
        FloorImage.color = FloorColor[CurFloor];
        WallTopImage.color = FloorColor[CurFloor];
        WallBotImage.color = FloorColor[CurFloor];
        */
        //光影
        if (!ShadowColor.ContainsKey(CurFloor))
        {
            ShadowColor.Add(CurFloor, GetCurShadowColor(CurFloor));
        }
        FloorShadow.color = GetColorByCurPlate(CurPlate, 1);
        WallTopShadowImage.color = FloorShadow.color;
        WallBotShadowImage.color = FloorShadow.color;
        /*
        FloorShadow.color = ShadowColor[CurFloor];
        WallTopShadowImage.color = ShadowColor[CurFloor];
        WallBotShadowImage.color = ShadowColor[CurFloor];
        */
        //線搞
        if (!LineDraftColor.ContainsKey(CurFloor))
        {
            LineDraftColor.Add(CurFloor, GetCurLineDraftColor(CurFloor));
        }
        FloorLineDraft.color = GetColorByCurPlate(CurPlate, 2);
        WallTopLineImage.color = FloorLineDraft.color;
        WallBotLineImage.color = FloorLineDraft.color;

        /*
        FloorLineDraft.color = LineDraftColor[CurFloor];
        WallTopLineImage.color = LineDraftColor[CurFloor];
        WallBotLineImage.color = LineDraftColor[CurFloor];
        */
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
