﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public bool Loop;
    public float LifeTime;
    static Dictionary<string, float> ParticleLifeTimeDic = new Dictionary<string, float>();

    private ParticleSystem ps;
    void Start()
    {
        if (ParticleLifeTimeDic.ContainsKey(name))
        {
            LifeTime = ParticleLifeTimeDic[name];
            if (LifeTime == 1000)
                Loop = true;
        }
        else
        {
            LifeTime = 0;
            Loop = false;
            ParticleSystem[] psArray = transform.GetComponentsInChildrenExcludeSelf<ParticleSystem>();
            for (int i = 0; i < psArray.Length; i++)
            {
                if (psArray[i].main.loop)
                {
                    Loop = true;
                    break;
                }
                float time = psArray[i].main.startDelayMultiplier + psArray[i].main.duration + psArray[i].main.startLifetimeMultiplier;
                if (LifeTime < time)
                    LifeTime = time;
            }
            ParticleLifeTimeDic.Add(name, LifeTime);
        }
        if (!Loop)
            StartCoroutine(WaitToDestroy(LifeTime));
    }
    IEnumerator WaitToDestroy(float _time)
    {
        yield return new WaitForSeconds(_time);
        Destroy(gameObject);
    }
}
