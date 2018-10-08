using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MyEnum : MonoBehaviour
{
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
