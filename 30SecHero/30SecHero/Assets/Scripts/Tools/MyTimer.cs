using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    float CurTimer;
    public float MaxTime { get; private set; }
    public bool StartRunTimer;
    public bool Loop;
    public string Key;
    public delegate void MyDelegate();
    public delegate void MyKeyDelegate(string _key);
    MyDelegate TimeOutFunc;
    MyDelegate RunTimeFunc;
    MyKeyDelegate TimeOutFuncWithKey;


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
    public MyTimer(float _maxTime, MyKeyDelegate _timeOutFunc, MyDelegate _runTimeFunc, bool _startRunTimer, bool _loop, string _key)
    {
        MaxTime = _maxTime;
        CurTimer = MaxTime;
        TimeOutFuncWithKey = _timeOutFunc;
        RunTimeFunc = _runTimeFunc;
        StartRunTimer = _startRunTimer;
        Loop = _loop;
        Key = _key;
        if (MaxTime == 0)
        {
            Debug.LogWarning(string.Format("{0}'s MaxTime of MyTimer is 0", _timeOutFunc.Method.Name));
        }
    }
    public MyTimer(float _maxTime, MyKeyDelegate _timeOutFunc, bool _startRunTimer, bool _loop, string _key)
    {
        MaxTime = _maxTime;
        CurTimer = MaxTime;
        TimeOutFuncWithKey = _timeOutFunc;
        StartRunTimer = _startRunTimer;
        Loop = _loop;
        Key = _key;
        if (MaxTime == 0)
        {
            Debug.LogWarning(string.Format("{0}'s MaxTime of MyTimer is 0", _timeOutFunc.Method.Name));
        }
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
            else if (TimeOutFuncWithKey != null)
                TimeOutFuncWithKey(Key);
        }
    }
}
