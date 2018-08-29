using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRole : Role
{
    public void Jump(Direction _dir)
    {
        switch(_dir)
        {
            case Direction.Top:
                transform.position += new Vector3(0, 30, 0);
                break;
            case Direction.Bottom:
                transform.position += new Vector3(0, -30, 0);
                break;
            case Direction.Right:
                transform.position += new Vector3(30, 0, 0);
                break;
            case Direction.Left:
                transform.position += new Vector3(-30, 0, 0);
                break;
        }
    }

}
