using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleManager : MonoBehaviour
{
    public ParticleSystem MyParticle;
    public bool Loop;
    public float LifeTime;
    float CurParticleTime = 0;
    static Dictionary<string, float> ParticleLifeTimeDic = new Dictionary<string, float>();
    AudioSource[] MyAudios;

    public void Init()
    {
        MyParticle = GetComponent<ParticleSystem>();
        var particleModule = GetComponent<ParticleSystem>().main;
        particleModule.playOnAwake = false;
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
            if (Loop)
                ParticleLifeTimeDic.Add(name, 1000);
            else
                ParticleLifeTimeDic.Add(name, LifeTime);
        }
    }

    void OnDisable()
    {
        if (MyParticle == null)
            return;
        CurParticleTime = MyParticle.time;
        if (!Loop)
            if (CurParticleTime > LifeTime)
                CurParticleTime = LifeTime;
        DisableAudio();
    }
    void DisableAudio()
    {
        MyAudios = GetComponentsInChildren<AudioSource>();
        if (MyAudios != null)
        {
            for (int i = 0; i < MyAudios.Length; i++)
            {
                MyAudios[i].enabled = false;
            }
        }
    }
    void OnEnable()
    {
        if (MyParticle == null)
            return;
        if (Loop || CurParticleTime < LifeTime)
        {
            MyParticle.Simulate(CurParticleTime);
            MyParticle.Play();
        }
    }
    void Update()
    {
        if (Loop)
            return;
        if (!MyParticle)
            return;
        CurParticleTime = MyParticle.time;
        if (CurParticleTime >= LifeTime)
            Destroy(gameObject);
    }
}
