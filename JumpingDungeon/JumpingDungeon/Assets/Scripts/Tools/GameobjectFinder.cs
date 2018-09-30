using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectFinder : MonoBehaviour
{
    public static GameObject SelfGameobject;
    public static GameObject FindClosestGameobjectWithTag(GameObject _self, string _tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        SelfGameobject = _self;
        Vector3 position = SelfGameobject.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    public static GameObject FindInRangeClosestGameobjectWithTag(GameObject _self, string _tag, int _range)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        SelfGameobject = _self;
        Vector3 position = SelfGameobject.transform.position;
        foreach (GameObject go in gos)
        {
            float curDistance = Vector3.Distance(go.transform.position, position);
            if (curDistance > _range)
                continue;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    public static List<GameObject> FindClosestGameobjectsWithTag(GameObject _self, string _tag, int _count)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_tag);
        SelfGameobject = _self;
        List<GameObject> goList = new List<GameObject>(gos);
        goList.Remove(SelfGameobject);
        goList.Sort(SortByDistance);
        List<GameObject> targetList = new List<GameObject>();
        for (int i = 0; i < _count; i++)
        {
            if (i < goList.Count)
                targetList.Add(goList[i]);
            else
                break;
        }
        return targetList;
    }
    public static List<GameObject> FindInRangeClosestGameobjectsWithTag(GameObject _self, string _tag, int _count, int _range)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_tag);
        SelfGameobject = _self;
        Vector3 position = SelfGameobject.transform.position;
        List<GameObject> goList = new List<GameObject>(gos);
        goList.Remove(SelfGameobject);
        goList.Sort(SortByDistance);
        List<GameObject> targetList = new List<GameObject>();
        for (int i = 0; i < _count; i++)
        {
            if (i < goList.Count)
            {
                float curDistance = Vector3.Distance(goList[i].transform.position, position);
                if (curDistance > _range)
                    break;
                else
                    targetList.Add(goList[i]);
            }
            else
                break;
        }
        return targetList;
    }
    static int SortByDistance(GameObject _go1, GameObject _go2)
    {
        float dstToA = Vector3.Distance(SelfGameobject.transform.position, _go1.transform.position);
        float dstToB = Vector3.Distance(SelfGameobject.transform.position, _go2.transform.position);
        return dstToA.CompareTo(dstToB);
    }
}
