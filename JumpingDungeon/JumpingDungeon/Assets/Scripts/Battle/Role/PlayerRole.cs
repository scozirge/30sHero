using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerRole : Role
{
    protected override void Awake()
    {
        base.Awake();
        BaseDamage = 100;
        MoveSpeed = 300;
    }
    protected override void Update()
    {
        base.Update();
        Controller();
    }
    public void Jump(Direction _dir)
    {
        switch(_dir)
        {
            case Direction.Top:
                transform.position += new Vector3(0, MoveSpeed, 0);
                break;
            case Direction.Bottom:
                transform.position += new Vector3(0, -MoveSpeed, 0);
                break;
            case Direction.Right:
                transform.position += new Vector3(MoveSpeed, 0, 0);
                break;
            case Direction.Left:
                transform.position += new Vector3(-MoveSpeed, 0, 0);
                break;
        }
    }
    

}
