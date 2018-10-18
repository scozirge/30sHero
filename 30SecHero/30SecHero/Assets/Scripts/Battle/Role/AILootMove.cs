using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILootMove : AIMove
{
    [Tooltip("移動速度")]
    [SerializeField]
    protected int MoveSpeed;
    protected override void Start()
    {
        base.Start();
        if (Destination == Vector2.zero)
        {
            Destination = transform.position;
        }
        else
            RandomOffset = Destination;
    }
    protected override void Debut()
    {
        base.Debut();
        if (KeepDebut || FollowCamera)
        {
            Vector2 targetVel = (Destination - (Vector2)transform.position).normalized * DebutSpeed;
            MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, targetVel, RotateFactor);
        }
    }
    protected override void WanderMovement()
    {
        if (!Wander)
            return;
        base.WanderMovement();
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
        WanderVelocity = (RandDestination - transform.position).normalized * MoveSpeed * 1.2f;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, WanderVelocity, RotateFactor);
    }
}
