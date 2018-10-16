using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct EffectData
{
    [Tooltip("特效名稱)")]
    [SerializeField]
    public string Name;
    [Tooltip("特效名稱)")]
    [SerializeField]
    public ParticleSystem Particle;
}
