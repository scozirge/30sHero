using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    float CurTimer;
    float MaxTime;
    bool StartRunTimer;
    public delegate void MyDelegate();
    MyDelegate TimeOutFunc;
    MyDelegate RunTimeFunc;

    public MyTimer(float _maxTime, MyDelegate _timeOutFunc, MyDelegate _RunTimeFunc)
    {
        MaxTime = _maxTime;
        CurTimer = MaxTime;
        TimeOutFunc = _timeOutFunc;
        RunTimeFunc = _RunTimeFunc;
        StartRunTimer = false;
        if (MaxTime == 0)
            Debug.LogWarning("MaxTime of MyTimer is 0");
    }
    public void Start(bool _startRunTimer)
    {
        StartRunTimer = _startRunTimer;
    }
    public void RunTimer()
    {
        if (MaxTime == 0)
            return;
        if (!StartRunTimer)
            return;
        if (RunTimeFunc != null)
            RunTimeFunc();
        if (CurTimer > 0)
            CurTimer -= Time.deltaTime;
        else
        {
            CurTimer = MaxTime;
            StartRunTimer = false;
            if (TimeOutFunc != null)
                TimeOutFunc();
        }
    }
}
