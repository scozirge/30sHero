using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct LootData
{
    public LootType Type;
    public float Time;
    public float Value;
    public LootData(LootType _type, float _time, float _value)
    {
        Type = _type;
        Time = _time;
        Value = _value;
    }
}
