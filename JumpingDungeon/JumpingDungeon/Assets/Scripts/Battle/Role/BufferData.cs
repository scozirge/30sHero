using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BufferData
{
    public RoleBuffer Type;
    public float Time;
    public float Value;

    public BufferData(RoleBuffer _type, float _time)
    {
        Type = _type;
        Time = _time;
    }
    public BufferData(RoleBuffer _type, float _time, float _value)
    {
        Type = _type;
        Time = _time;
        Value = _value;
    }
}
