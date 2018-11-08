using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SpawnEnemyData
{
    [Tooltip("召喚怪物")]
    [SerializeField]
    public EnemyRole Enemy;
    [Tooltip("召喚位置相對於哪個目標")]
    [SerializeField]
    public PosRelateTo SpawnPosRelateTo;
    [Tooltip("位置")]
    [SerializeField]
    public Vector2 SpawnPosition;
    [Tooltip("召喚物存活時間")]
    [SerializeField]
    public float LifeTime;
    [HideInInspector]
    public bool ExpandFolder;
}
