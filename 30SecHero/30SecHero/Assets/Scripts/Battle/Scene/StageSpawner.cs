using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField]
    List<Stage> StageList;
    [SerializeField]
    Stage DesignateStage;
    [SerializeField]
    List<ForeGround> FGTopList;
    [SerializeField]
    List<ForeGround> FGBotList;

    static StageSpawner MyStageSpawner;

    public void Init()
    {
        MyStageSpawner = this;
    }
    static Stage GetRandomStage(ref int _remainPlateSize)
    {
        for (int i = 0; i < 30; i++)
        {
            int random = Random.Range(0, MyStageSpawner.StageList.Count);
            if (_remainPlateSize >= MyStageSpawner.StageList[random].OccupyPlateSize)
            {
                _remainPlateSize -= MyStageSpawner.StageList[random].OccupyPlateSize;
                return MyStageSpawner.StageList[random];
            }
        }
        return null;
    }
    public static List<Stage> SpawnStage(Vector2 _starPos, int _offsetPosX, int _remainPlateSize, int _floor)
    {
        List<Stage> list = new List<Stage>();
        int originalSize = _remainPlateSize;
        for (int i = 0; i < 10; i++)
        {
            Vector2 spawnPos = new Vector2(_starPos.x + _offsetPosX * (originalSize - _remainPlateSize), _starPos.y);
            Stage stage = null;
            if (MyStageSpawner.DesignateStage)
            {
                Stage stagePrefab = MyStageSpawner.DesignateStage;
                _remainPlateSize -= MyStageSpawner.DesignateStage.OccupyPlateSize;
                if (stagePrefab == null)
                    break;
                stage = Instantiate(stagePrefab, Vector3.zero, Quaternion.identity) as Stage;
            }
            else
            {
                Stage stagePrefab = GetRandomStage(ref _remainPlateSize);
                if (stagePrefab == null)
                    break;
                stage = Instantiate(stagePrefab, Vector3.zero, Quaternion.identity) as Stage;
            }
            stage.transform.SetParent(MyStageSpawner.transform);
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
        int rand = Random.Range(0, MyStageSpawner.FGTopList.Count);
        return MyStageSpawner.FGTopList[rand];
    }
    static ForeGround GetRandomBotFG()
    {
        int rand = Random.Range(0, MyStageSpawner.FGBotList.Count);
        return MyStageSpawner.FGBotList[rand];
    }
    public static List<ForeGround> SpawnFG(Vector2 _startPos, float _distance, int _minXInterval, int _maxXInterval, int _floor, bool _top)
    {
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
                fg.transform.SetParent(MyStageSpawner.transform);
                fg.transform.localPosition = spawnPos;
                fg.Init(_floor);
                list.Add(fg);
            }
        }
        return list;
    }

}
