using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Loot : MonoBehaviour {

    [Tooltip("在可以吃掉前需求的等待時間")]
    [SerializeField]
    protected float WaitToBeAcquire = 0;
    [Tooltip("掉落隨機位置半徑")]
    [SerializeField]
    protected int RandomPosRadius = 0;
    AILootMove MyAIMove;
    protected bool ReadyToAcquire;


    protected virtual void Start()
    {
        MyAIMove = GetComponent<AILootMove>();
        if (MyAIMove)
            MyAIMove.ReadyToMove = false;
        RandomPos();
        StartCoroutine(WaitToMoveToAcquire());
    }
    void RandomPos()
    {
        if (RandomPosRadius == 0)
            return;
        int randX = Random.Range(-RandomPosRadius, RandomPosRadius);
        int randY = Random.Range(-RandomPosRadius, RandomPosRadius);
        transform.position += new Vector3(randX, randY);
    }

    IEnumerator WaitToMoveToAcquire()
    {
        yield return new WaitForSeconds(WaitToBeAcquire);
        ReadyToAcquire = true;
        if (MyAIMove)
            MyAIMove.ReadyToMove = true;
    }
}
