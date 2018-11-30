using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField]
    bool TestMode;
    [SerializeField]
    List<Stage> StageList;
    [SerializeField]
    List<ForeGround> FGTopList;
    [SerializeField]
    List<ForeGround> FGBotList;

    static StageSpawner Myself;

    public void Init()
    {
        Myself = this;
    }
    static void ReallocateStageList(int _floor)
    {
        if (Myself.TestMode)
            return;
        Myself.StageList.Clear();
        Myself.StageList = StageData.GetAvailableStages(_floor);
        Debug.Log(Myself.StageList.Count);
    }
    static Stage GetRandomStage(ref int _remainPlateSize)
    {
        for (int i = 0; i < 30; i++)
        {
            int random = Random.Range(0, Myself.StageList.Count);
            if (_remainPlateSize >= Myself.StageList[random].OccupyPlateSize)
            {
                _remainPlateSize -= Myself.StageList[random].OccupyPlateSize;
                return Myself.StageList[random];
            }
        }
        return null;
    }
    public static List<Stage> SpawnStage(Vector2 _starPos, int _offsetPosX, int _remainPlateSize, int _floor)
    {
        List<Stage> list = new List<Stage>();
        ReallocateStageList(_floor);
        int originalSize = _remainPlateSize;
        for (int i = 0; i < originalSize; i++)
        {
            Vector2 spawnPos = new Vector2(_starPos.x + _offsetPosX * (originalSize - _remainPlateSize), _starPos.y);
            Stage stage = null;
            if (Myself.StageList.Count != 0)
            {
                Stage stagePrefab = GetRandomStage(ref _remainPlateSize);
                if (stagePrefab == null)
                    break;
                stage = Instantiate(stagePrefab, Vector3.zero, Quaternion.identity) as Stage;
            }
            else
                return null;
            stage.transform.SetParent(Myself.transform);
            stage.transform.localPosition = spawnPos;
            stage.Init(_floor);
            list.Add(stage);
            if (_remainPlateSize <= 0)
                break;
        }
        return list;
    }
    static ForeGround GetRandomTopFG()
    {
        int rand = Random.Range(0, Myself.FGTopList.Count);
        return Myself.FGTopList[rand];
    }
    static ForeGround GetRandomBotFG()
    {
        int rand = Random.Range(0, Myself.FGBotList.Count);
        return Myself.FGBotList[rand];
    }
    public static List<ForeGround> SpawnFG(Vector2 _startPos, float _distance, int _minXInterval, int _maxXInterval, int _floor, bool _top)
    {
        if (_top)
        {
            if (Myself.FGTopList.Count == 0)
            {
                return null;
            }
        }
        else
        {
            if (Myself.FGBotList.Count == 0)
            {
                return null;
            }
        }
        List<ForeGround> list = new List<ForeGround>();
        float curPosX = 0;
        float runDistance = 0;
        for (int i = 0; i < 100; i++)
        {
            int randomX = Random.Range(_minXInterval, _maxXInterval);
            runDistance += randomX;
            curPosX = runDistance + _startPos.x;
            if (runDistance > _distance)
            {
                break;
            }
            else
            {
                Vector2 spawnPos = new Vector2(curPosX, _startPos.y);
                ForeGround stagePrefab;
                if (_top)
                    stagePrefab = GetRandomTopFG();
                else
                    stagePrefab = GetRandomBotFG();
                if (stagePrefab == null)
                    break;
                ForeGround fg = Instantiate(stagePrefab, Vector3.zero, Quaternion.identity) as ForeGround;
                fg.transform.SetParent(Myself.transform);
                fg.transform.localPosition = spawnPos;
                fg.Init(_floor);
                list.Add(fg);
            }
        }
        return list;
    }

}
