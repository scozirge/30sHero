using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loot : MonoBehaviour {

    [Tooltip("在可以吃掉前需求的等待時間")]
    [SerializeField]
    protected float WaitToBeAcquire = 0;
    AILootMove MyAIMove;
    protected bool ReadyToAcquire;


    protected virtual void Start()
    {
        MyAIMove = GetComponent<AILootMove>();
        if (MyAIMove)
            MyAIMove.ReadyToMove = false;
        StartCoroutine(WaitToMoveToAcquire());
    }

    IEnumerator WaitToMoveToAcquire()
    {
        yield return new WaitForSeconds(WaitToBeAcquire);
        ReadyToAcquire = true;
        if (MyAIMove)
            MyAIMove.ReadyToMove = true;
    }
}
