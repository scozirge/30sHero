using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerRole
{
    const int MoveFactor = 1;
    void Controller()
    {
        if (Conditions.ContainsKey(RoleCondition.Stun))
            return;
        float xMoveForce = Input.GetAxis("Horizontal") * MoveSpeed * MoveFactor;
        float yMoveForce = Input.GetAxis("Vertical") * MoveSpeed * MoveFactor;
        MyRigi.velocity += new Vector2(xMoveForce, yMoveForce);
    }
}
