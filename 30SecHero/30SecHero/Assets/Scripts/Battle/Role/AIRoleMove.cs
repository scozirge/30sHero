using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyRole))]
public class AIRoleMove : AIMove
{

    Vector3 InitialVelocity;
    EnemyRole ER;

    protected override void Start()
    {
        base.Start();
        if (!MoveToPlayer)
        {
            ER = GetComponent<EnemyRole>();
            //int randX = Random.Range(0, 800);
            //int randY = Random.Range(-400, 400);

            if (Destination == Vector2.zero)
            {
                SetRandDestination();
            }
            else
                RandomOffset = Destination;
            //Vector3 rndTarget = new Vector3(randX, randY) + BattleManage.MyCameraControler.transform.position;
            Vector3 rndTarget = new Vector3(Destination.x, Destination.y);
            InitialVelocity = (rndTarget - transform.position).normalized * DebutSpeed;
            MyRigi.velocity = InitialVelocity;
        }
    }

    protected override void Debut()
    {
        base.Debut();

        if (KeepDebut || FollowCamera)
        {
            Vector2 targetVel = (Destination - (Vector2)transform.position).normalized * ER.MoveSpeed;
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
        WanderVelocity = (RandDestination - transform.position).normalized * ER.MoveSpeed * 1.2f;
        MyRigi.velocity = Vector2.Lerp(MyRigi.velocity, WanderVelocity, RotateFactor);
    }
}
