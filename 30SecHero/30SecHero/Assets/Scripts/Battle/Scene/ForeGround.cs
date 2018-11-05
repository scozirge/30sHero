using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForeGround : MonoBehaviour
{
    [HideInInspector]
    public int Floor;
    public void Init(int _floor)
    {
        Floor = _floor;
    }
}
