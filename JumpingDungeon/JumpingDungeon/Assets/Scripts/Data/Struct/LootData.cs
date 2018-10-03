using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct LootData
{
    public LootType Type;
    public float Time;
    public float Value;
    public Sprite LootIcon;
    [Tooltip("吃到道具後產身在腳色身上的特效)")]
    [SerializeField]
    public ParticleSystem GetParticle;
}
