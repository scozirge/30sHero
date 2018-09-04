using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyRole))]
[RequireComponent(typeof(Rigidbody2D))]
public class AIMove : MonoBehaviour
{
    [SerializeField]
    bool Wander;
    [SerializeField]
    float WanderInterval;
    [SerializeField]
    int RandWanderRangeX;
    [SerializeField]
    int RandWanderRangeY;
    [SerializeField]
    float DebutRotateFactor;
    [SerializeField]
    float InRangeStartWander;


    Vector3 Destination;
    float WanderIntervalTimer;
    Vector3 InitialVelocity;
    Vector3 WanderVelocity;
    Vector3 RandDestination;
    bool StartWander;
    Rigidbody2D MyRigi;
    EnemyRole ER;

    void Start()
    {
        ER = GetComponent<EnemyRole>();
        MyRigi = GetComponent<Rigidbody2D>();
        CameraController cc = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        WanderIntervalTimer = WanderInterval;

        int randX = Random.Range(-800, 800);
        int randY = Random.Range(-400, 400);
        Vector3 rndTarget = new Vector3(randX, randY) + cc.transform.position;
        InitialVelocity = (rndTarget - transform.position).normalized * ER.MoveSpeed;
        MyRigi.velocity = InitialVelocity;
        Vector3 cameraPos = cc.transform.position;
        Vector3 screenSize = cc.ScreenSize;
        float randPosX = Random.Range(-screenSize.x / 2, screenSize.x / 2) + cc.transform.position.x;
        float randPosY = Random.Range(-screenSize.y / 2, screenSize.y / 2) + cc.transform.position.y;
        Destination = new Vector3(randPosX, randPosY, 0);
    }

    public void Debut()
    {
        if (StartWander)
            return;
        Vector2 targetVel = (Destination - transform.position).normalized * ER.MoveSpeed;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, targetVel, DebutRotateFactor);

    }

    void WanderTimerFunc()
    {
        if (!Wander)
            return;

        if (!StartWander)
        {
            float dist = Mathf.Abs(Vector2.Distance(Destination, transform.position));
            if (InRangeStartWander > dist)
            {
                StartWander = true;
                CalculateRandDestination();
            }
            return;
        }


        if (WanderIntervalTimer > 0)
            WanderIntervalTimer -= Time.deltaTime;
        else
        {
            WanderIntervalTimer = WanderInterval;
            CalculateRandDestination();
        }
    }
    void CalculateRandDestination()
    {
        int randX = Random.Range(-RandWanderRangeX, RandWanderRangeX);
        int randY = Random.Range(-RandWanderRangeY, RandWanderRangeY);
        RandDestination = new Vector3(randX, randY) + Destination;
    }
    void WanderMovement()
    {
        if (!StartWander)
            return;

        WanderVelocity = (RandDestination - transform.position).normalized * ER.MoveSpeed;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, WanderVelocity, DebutRotateFactor);
    }

    void Update()
    {
        Debut();
        WanderTimerFunc();
        WanderMovement();
    }


}
