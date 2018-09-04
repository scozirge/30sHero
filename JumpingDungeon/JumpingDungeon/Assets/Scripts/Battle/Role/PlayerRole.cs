using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerRole : Role
{
    const int MoveFactor = 1;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void Move()
    {
        base.Move();
        if (Buffers.ContainsKey(RoleBuffer.Stun))
            return;
        float xMoveForce = Input.GetAxis("Horizontal") * MoveSpeed * MoveFactor;
        float yMoveForce = Input.GetAxis("Vertical") * MoveSpeed * MoveFactor;
        MyRigi.velocity += new Vector2(xMoveForce, yMoveForce);
    }
    

}
