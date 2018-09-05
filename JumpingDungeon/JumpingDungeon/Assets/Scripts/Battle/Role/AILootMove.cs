using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AILootMove : MonoBehaviour
{
    [SerializeField]
    int MoveSpeed;
    [SerializeField]
    bool FollowCamera;
    [SerializeField]
    float WanderInterval;
    [SerializeField]
    float RotateFactor;
    [SerializeField]
    float WanderRange;



    static CameraController CC;
    Vector3 Destination;
    Vector3 CameraPos;
    float WanderIntervalTimer;
    Vector3 WanderVelocity;
    Vector3 RandDestination;
    Rigidbody2D MyRigi;
    Vector3 OriginalPos;

    void Start()
    {
        MyRigi = GetComponent<Rigidbody2D>();
        if (!CC)
            CC = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        WanderIntervalTimer = WanderInterval;
        OriginalPos = transform.position;
        Destination = OriginalPos;
        //Set Target Position
        CalculateRandDestination();
    }

    void WanderTimerFunc()
    {
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
        if (FollowCamera)
        {
            //Follow camera
            CameraPos = CC.transform.position;
            Destination = new Vector3(CameraPos.x, CameraPos.y, 0) + OriginalPos;
        }
        WanderVelocity = (RandDestination - transform.position).normalized * MoveSpeed;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, WanderVelocity, RotateFactor);

    }
    void CalculateRandDestination()
    {
        RandDestination = new Vector3(Random.Range(-WanderRange, WanderRange), Random.Range(-WanderRange, WanderRange)) + Destination;
    }
    void Update()
    {
        WanderTimerFunc();
        WanderMovement();
    }


}
