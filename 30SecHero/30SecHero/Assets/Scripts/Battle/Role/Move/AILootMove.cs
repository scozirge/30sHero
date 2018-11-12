using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILootMove : AIMove
{
    [Tooltip("移動速度")]
    [SerializeField]
    protected int MoveSpeed;
    [Tooltip("靠近玩家自動吸過去半徑")]
    [SerializeField]
    protected int AbsorbRadius;

    void OnDrawGizmos()
    {
        if (AbsorbRadius <= 0)
            return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AbsorbRadius);
    }
    protected override void Update()
    {
        base.Update();
        if (!MoveToPlayer && AbsorbRadius > 0)
        {
            if (Vector2.Distance(transform.position, BattleManage.BM.MyPlayer.transform.position) <= AbsorbRadius)
                MoveToPlayer = true;
        }
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Debut()
    {
        base.Debut();
        if (KeepDebut || FollowCamera)
        {
            Vector2 targetVel = (Destination - (Vector2)transform.position).normalized * FollowCameraSpeed;
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
