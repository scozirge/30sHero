using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    float CurTimer;
    float MaxTime;
    public bool StartRunTimer;
    public bool Loop;
    public delegate void MyDelegate();
    MyDelegate TimeOutFunc;
    MyDelegate RunTimeFunc;

    public MyTimer(float _maxTime, MyDelegate _timeOutFunc, bool _startRunTimer, bool _loop)
    {
        MaxTime = _maxTime;
        CurTimer = MaxTime;
        TimeOutFunc = _timeOutFunc;
        StartRunTimer = _startRunTimer;
        Loop = _loop;
        
        if (MaxTime == 0)
        {
            Debug.LogWarning(string.Format("{0}'s MaxTime of MyTimer is 0", _timeOutFunc.Method.Name));
        }

    }

    public MyTimer(float _maxTime, MyDelegate _timeOutFunc, MyDelegate _runTimeFunc, bool _startRunTimer, bool _loop)
    {
        MaxTime = _maxTime;
        CurTimer = MaxTime;
        TimeOutFunc = _timeOutFunc;
        RunTimeFunc = _runTimeFunc;
        StartRunTimer = _startRunTimer;
        Loop = _loop;
        if (MaxTime == 0)
            Debug.LogWarning("MaxTime of MyTimer is 0");
    }
    public void RestartCountDown()
    {
        CurTimer = MaxTime;
    }
    public void ResetMaxTime(float _maxTime)
    {
        MaxTime = _maxTime;
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
            StartRunTimer = Loop;
            if (TimeOutFunc != null)
                TimeOutFunc();
        }
    }
}
