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
    bool FollowCamera;
    [SerializeField]
    float WanderInterval;
    [SerializeField]
    float DebutRotateFactor;
    [SerializeField]
    float WanderRange;
    [SerializeField]
    Vector3 Destination;



    static float InRangeStartWander = 50;

    Vector3 CameraPos;
    Vector3 CameraSize;
    Vector3 RandomOffset;
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
        WanderIntervalTimer = WanderInterval;
        int randX = Random.Range(0, 800);
        int randY = Random.Range(-400, 400);
        Vector3 rndTarget = new Vector3(randX, randY) + BattleManage.MyCameraControler.transform.position;
        InitialVelocity = (rndTarget - transform.position).normalized * ER.MoveSpeed;
        MyRigi.velocity = InitialVelocity;

        //Set Target Position
        CameraPos = BattleManage.MyCameraControler.transform.position;
        CameraSize = BattleManage.ScreenSize;
        float randPosX = Random.Range(100, CameraSize.x / 2);
        float randPosY = Random.Range(-CameraSize.y / 2 + 100, CameraSize.y / 2 - 100);
        RandomOffset = new Vector2(randPosX, randPosY);
        Destination = new Vector3(randPosX + CameraPos.x, randPosY + CameraPos.y, 0);

    }

    public void Debut()
    {
        if (FollowCamera)
        {
            //Follow camera
            CameraPos = BattleManage.MyCameraControler.transform.position;
            Destination = new Vector3(CameraPos.x, CameraPos.y, 0) + RandomOffset;
        }

        Vector2 targetVel = (Destination - transform.position).normalized * ER.MoveSpeed;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, targetVel, DebutRotateFactor);
    }

    void WanderTimerFunc()
    {
        if (!Wander)
            return;
        if (!StartWander)
            return;
        if (WanderIntervalTimer > 0)
            WanderIntervalTimer -= Time.deltaTime;
        else
        {
            WanderIntervalTimer = WanderInterval;
            CalculateRandDestination();
        }
    }
    void WanderMovement()
    {
        if (!StartWander)
        {
            float dist = Mathf.Abs(Vector2.Distance(Destination, transform.position));
            if (dist < InRangeStartWander)
            {
                StartWander = true;
                CalculateRandDestination();
            }
            return;
        }
        WanderVelocity = (RandDestination - transform.position).normalized * ER.MoveSpeed * 1.2f;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, WanderVelocity, DebutRotateFactor);
    }
    void CalculateRandDestination()
    {
        RandDestination = new Vector3(Random.Range(-WanderRange, WanderRange), Random.Range(-WanderRange, WanderRange)) + Destination;
    }
    void Update()
    {
        Debut();
        WanderTimerFunc();
        WanderMovement();
    }


}
