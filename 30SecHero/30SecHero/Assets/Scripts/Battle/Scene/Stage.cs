using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    public int OccupyPlateSize;
    [HideInInspector]
    public int Floor;
    public void Init(int _floor)
    {
        Floor = _floor;
    }
}
