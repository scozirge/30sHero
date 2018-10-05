using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSpawner : MonoBehaviour
{
    [SerializeField]
    CameraController FollowCamera;
    [SerializeField]
    float IntervalDistX;
    [SerializeField]
    float IntervalDistY;
    [SerializeField]
    int ConstraintCountX;
    [SerializeField]
    int ConstraintCountY;
    [SerializeField]
    Vector2 StartPos;
    [SerializeField]
    Plate Prefab;

    List<Plate> PlateList = new List<Plate>();
    int OffsetLevel;
    int OrigOffsetLevel;

    void Start()
    {
        InitSpawnBG();
    }
    void InitSpawnBG()
    {
        for (int i = 0; i < ConstraintCountX; i++)
        {
            for (int j = 0; j < ConstraintCountY; j++)
            {
                GameObject go = Instantiate(Prefab.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
                Plate plate = go.GetComponent<Plate>();
                plate.Init(i, ConstraintCountX);
                plate.transform.SetParent(transform);
                plate.transform.localPosition = Vector2.zero;
                plate.transform.localPosition += new Vector3(IntervalDistX * i - ConstraintCountX / 2 * IntervalDistX, IntervalDistY * j - ConstraintCountY / 2 * IntervalDistY, 0);
                PlateList.Add(plate);
            }
        }
    }
    void Update()
    {
        AdjustPos();
    }
    void AdjustPos()
    {
        float cameraX=FollowCamera.transform.position.x;
        if (cameraX>0)
            OffsetLevel = Mathf.FloorToInt(cameraX / IntervalDistX);
        else
            OffsetLevel = Mathf.CeilToInt(cameraX / IntervalDistX);
        if (OrigOffsetLevel != OffsetLevel)
        {
            if (OffsetLevel < OrigOffsetLevel)
            {
                for (int i = 0; i < PlateList.Count; i++)
                {
                    if (PlateList[i].ColumnRank == ConstraintCountX - 1)
                        PlateList[i].transform.localPosition -= new Vector3(IntervalDistX * ConstraintCountX, 0);
                    PlateList[i].LevelDown();
                }
            }
            else
            {

                for (int i = 0; i < PlateList.Count; i++)
                {
                    if (PlateList[i].ColumnRank == 0)
                        PlateList[i].transform.localPosition += new Vector3(IntervalDistX * ConstraintCountX, 0);
                    PlateList[i].LevelUp();
                }
            }
            OrigOffsetLevel = OffsetLevel;
        }
    }
}
