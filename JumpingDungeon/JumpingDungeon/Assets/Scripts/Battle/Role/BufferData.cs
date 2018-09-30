using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferData
{
    public float Time;
    public float Value;

    public BufferData(float _time)
    {
        Time = _time;
    }
    public BufferData(float _time,float _value)
    {
        Time = _time;
        Value = _value;
    }
}
