using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectFinder : MonoBehaviour
{
    public static GameObject FindClosestGameobjectWithTag(GameObject _self, string _tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(_tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = _self.transform.position;
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
}
