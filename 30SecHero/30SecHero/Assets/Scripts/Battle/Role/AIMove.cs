using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AIMove : MonoBehaviour {
    [Tooltip("移動到指定座標後會不會遊蕩")]
    [SerializeField]
    protected bool Wander;
    [Tooltip("是否要跟著攝影機")]
    [SerializeField]
    protected bool FollowCamera;
    [Tooltip("遊蕩時間間隔")]
    [SerializeField]
    protected float WanderInterval;
    [Tooltip("轉向係數")]
    [SerializeField]
    protected float RotateFactor;
    [Tooltip("遊蕩範圍")]
    [SerializeField]
    protected float WanderRange;
    [SerializeField]
    public Vector2 Destination;

    protected bool CanMove;
    protected static float InRangeStartWander = 50;
    protected Vector3 RandomOffset;
    protected float WanderIntervalTimer;
    protected Vector3 WanderVelocity;
    protected Vector3 RandDestination;
    protected bool StartWander;
    protected Rigidbody2D MyRigi;
    protected bool KeepDebut;



 

    protected virtual void Start()
    {
        MyRigi = GetComponent<Rigidbody2D>();
        WanderIntervalTimer = WanderInterval;
        if (RotateFactor < 0.02f)
            RotateFactor = 0.02f;
        KeepDebut = true;
        CanMove = true;
    }
    public Vector2 SetRandDestination()
    {
        float randPosX = Random.Range(100, BattleManage.ScreenSize.x / 2);
        float randPosY = Random.Range(-BattleManage.ScreenSize.y / 2 + 100, BattleManage.ScreenSize.y / 2 - 100);
        RandomOffset = new Vector2(randPosX, randPosY);
        Vector2 cameraPos = BattleManage.MyCameraControler.transform.position;
        Destination = new Vector3(randPosX + cameraPos.x, randPosY + cameraPos.y, 0);
        return RandomOffset;
    }
    protected virtual void Debut()
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
    protected virtual void WanderMovement()
    {
    }
    protected void CalculateRandDestination()
    {
        RandDestination = new Vector2(Random.Range(-WanderRange, WanderRange), Random.Range(-WanderRange, WanderRange)) + Destination;
    }
    void FixedUpdate()
    {
        if (CanMove)
        {
            Debut();
            WanderMovement();
        }
    }
    public void SetCanMove(bool _bool)
    {
        CanMove = _bool;
        if (!CanMove)
            MyRigi.velocity = Vector3.zero;
    }
    void Update()
    {
        if (CanMove)
        {
            WanderTimerFunc();
        }
    }
}
