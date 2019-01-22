using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AIMove : MonoBehaviour
{
    [Tooltip("是否直接移動到目標身上，移動速度是看DebutSpeed")]
    [SerializeField]
    protected bool MoveToTarget;
    [SerializeField]
    protected string TargetTag;
    protected Transform TargetTrans;
    [Tooltip("移動到指定座標後會不會遊蕩")]
    [SerializeField]
    protected bool Wander;
    [Tooltip("是否要跟著攝影機")]
    [SerializeField]
    protected bool FollowCamera;
    [Tooltip("跟隨螢幕速度")]
    [SerializeField]
    protected int FollowCameraSpeed = 1000;
    [Tooltip("進場速度")]
    [SerializeField]
    public int DebutSpeed;
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
    [Tooltip("初始隨機力道")]
    [SerializeField]
    Vector2 RandomForce;

    protected bool CanMove;
    protected static float InRangeStartWander = 50;
    protected Vector3 RandomOffset;
    protected float WanderIntervalTimer;
    protected Vector3 WanderVelocity;
    protected Vector3 RandDestination;
    protected bool StartWander;
    protected Rigidbody2D MyRigi;
    [SerializeField]
    protected bool KeepDebut;
    [HideInInspector]
    public bool ReadyToMove = true;


    protected virtual void Start()
    {
        MyRigi = GetComponent<Rigidbody2D>();
        if (TargetTag != "")
        {
            GameObject go = GameObject.FindGameObjectWithTag(TargetTag);
            if (go != null)
                TargetTrans = go.transform;
        }

        if (ReadyToMove && MoveToTarget)
        {
            //MyRigi.velocity = new Vector2(Random.Range(-1200, 1200), Random.Range(-1200, 1200));
        }
        else
        {
            WanderIntervalTimer = WanderInterval;
            KeepDebut = true;
        }
        if (Destination == Vector2.zero)
        {
            SetHereToDestination();
        }
        if (RandomForce!=Vector2.zero)
        {
            float x = Random.Range(-RandomForce.x, RandomForce.x);
            float y = Random.Range(-RandomForce.y, RandomForce.y);
            RandomForce = new Vector2(x, y);
            MyRigi.AddForce(RandomForce);
        }
        CanMove = true;
    }
    public void SetHereToDestination()
    {
        Destination = transform.position;
        RandomOffset = Destination;
    }
    public Vector2 SetRandDestination()
    {
        float randPosX = Random.Range(-BattleManage.ScreenSize.x / 2, BattleManage.ScreenSize.x / 2);
        float randPosY = Random.Range(-BattleManage.ScreenSize.y / 2 + 200, BattleManage.ScreenSize.y / 2 - 200);
        RandomOffset = new Vector2(randPosX, randPosY);
        Vector2 camPos = Vector2.zero;
        camPos = BattleManage.MyCameraControler.transform.position;
        Destination = new Vector3(randPosX + camPos.x, randPosY + camPos.y, 0);
        return RandomOffset;
    }
    public Vector2 SetRandOutSideDestination(bool _fore)
    {

        int dir = 1;
        if (!_fore)
            dir = -1;
        float halfScreenSize = BattleManage.ScreenSize.x / 2;
        float randPosX = Random.Range(-halfScreenSize, halfScreenSize);
        float randPosY = Random.Range(-BattleManage.ScreenSize.y / 2 + 200, BattleManage.ScreenSize.y / 2 - 200);
        RandomOffset = new Vector2(randPosX, randPosY);
        Vector2 camPos = Vector2.zero;
        float randDesX = Random.Range((halfScreenSize + BattleManage.DisableMarginLengh) * dir, halfScreenSize * 3 * dir);
        float randDesY = Random.Range(-BattleManage.ScreenSize.y / 2 + 200, BattleManage.ScreenSize.y / 2 - 200);
        camPos = BattleManage.MyCameraControler.transform.position;
        Destination = new Vector3(randDesX + camPos.x, randDesY + camPos.y, 0);
        FollowCamera = false;
        return RandomOffset;
    }
    protected bool InDestinationRange;
    protected virtual void Debut()
    {
        if (FollowCamera)
        {
            //Follow camera
            Vector2 cameraPos = BattleManage.MyCameraControler.transform.position;
            Destination = cameraPos + (Vector2)RandomOffset;
        }
        if (!InDestinationRange)
        {
            if (Vector3.Distance(Destination, transform.position) < 50)
            {
                InDestinationRange = true;
                MyRigi.velocity *= 0.3f;
            }
        }

        if (KeepDebut)
            if (Vector3.Distance(Destination, transform.position) < 100)
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
            if (MoveToTarget)
            {
                MoveToTargetFunc();
            }
            else
            {
                Debut();
                WanderMovement();
            }
        }
    }
    void MoveToTargetFunc()
    {
        if (TargetTrans != null && ReadyToMove)
            MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, (TargetTrans.position - transform.position).normalized * DebutSpeed, 0.1f);
    }
    public void SetCanMove(bool _bool)
    {
        CanMove = _bool;
    }
    protected virtual void Update()
    {
        if (CanMove)
        {
            if (!MoveToTarget)
            {
                WanderTimerFunc();
            }
        }
    }
}
