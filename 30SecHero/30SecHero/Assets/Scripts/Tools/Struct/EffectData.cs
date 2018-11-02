using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct EffectData
{
    [Tooltip("特效名稱")]
    [SerializeField]
    public string Name;
    [Tooltip("特效物件")]
    [SerializeField]
    public ParticleSystem Particle;
    [Tooltip("產生在目標身上")]
    [SerializeField]
    public Transform TargetParent;
    [Tooltip("產生相對位置")]
    [SerializeField]
    public Vector2 Offset;
    [Tooltip("旋轉")]
    [SerializeField]
    public Vector2 Roration;
}
