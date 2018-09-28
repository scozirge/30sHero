using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyRole))]
[RequireComponent(typeof(Rigidbody2D))]
public class AIMove : MonoBehaviour
{
    [Tooltip("移動到指定座標後會不會遊蕩")]
    [SerializeField]
    bool Wander;
    [Tooltip("是否要跟著攝影機")]
    [SerializeField]
    bool FollowCamera;
    [Tooltip("遊蕩時間間隔")]
    [SerializeField]
    float WanderInterval;
    [Tooltip("轉向係數")]
    [SerializeField]
    float RotateFactor;
    [Tooltip("遊蕩範圍")]
    [SerializeField]
    float WanderRange;
    [SerializeField]
    public Vector2 Destination;



    static float InRangeStartWander = 50;

    Vector3 RandomOffset;
    float WanderIntervalTimer;
    Vector3 InitialVelocity;
    Vector3 WanderVelocity;
    Vector3 RandDestination;
    bool StartWander;
    Rigidbody2D MyRigi;
    EnemyRole ER;
    bool KeepDebut;

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
        if (RotateFactor < 0.02f)
            RotateFactor = 0.02f;
        KeepDebut = true;
        if (Destination == Vector2.zero)
        {
            SetRandDestination();
        }

    }
    public Vector2 SetRandDestination()
    {
        Vector2 RandomOffset = Vector2.zero;
        float randPosX = Random.Range(100, BattleManage.ScreenSize.x / 2);
        float randPosY = Random.Range(-BattleManage.ScreenSize.y / 2 + 100, BattleManage.ScreenSize.y / 2 - 100);
        RandomOffset = new Vector2(randPosX, randPosY);
        Vector2 cameraPos = BattleManage.MyCameraControler.transform.position;
        Destination = new Vector3(randPosX + cameraPos.x, randPosY + cameraPos.y, 0);
        return RandomOffset;
    }

    public void Debut()
    {
        if (FollowCamera)
        {
            //Follow camera
            Vector2 cameraPos = BattleManage.MyCameraControler.transform.position;
            Destination = new Vector3(cameraPos.x, cameraPos.y, 0) + RandomOffset;
        }

        if (!Wander && KeepDebut)
            if (Mathf.Abs(Vector3.Distance(Destination, transform.position)) < 30)
            {
                KeepDebut = false;
                MyRigi.velocity = Vector3.zero;
            }

        if (KeepDebut || FollowCamera)
        {
            Vector2 targetVel = (Destination - (Vector2)transform.position).normalized * ER.MoveSpeed;
            MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, targetVel, RotateFactor);
        }
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
        if (!Wander)
            return;
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
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, WanderVelocity, RotateFactor);
    }
    void CalculateRandDestination()
    {
        RandDestination = new Vector2(Random.Range(-WanderRange, WanderRange), Random.Range(-WanderRange, WanderRange)) + Destination;
    }
    void FixedUpdate()
    {
        Debut();
        WanderMovement();
    }
    void Update()
    {
        WanderTimerFunc();
    }
}
