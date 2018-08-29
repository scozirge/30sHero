using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{

    [SerializeField]
    float AngularVlocity = 3f;
    [SerializeField]
    float Radius;
    [SerializeField]
    Transform Arrow;
    [SerializeField]
    Transform Center;
    [SerializeField]
    PlayerRole PRole;

    protected float CurRadian;

    void Update()
    {
        CircularMotion();
        MouseFunc();
    }

    void CircularMotion()
    {
        CurRadian += AngularVlocity * Time.deltaTime;
        float x = Radius * Mathf.Cos(CurRadian);
        float y = Radius * Mathf.Sin(CurRadian);
        Arrow.localPosition = new Vector2(x, y);
        //Arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 90 - MyMath.GetAngerFormTowPoint2D(ShootPos, transform.position)));
    }
    float GetAngle()
    {
        return MyMath.GetAngerFormTowPoint2D(Arrow.localPosition, Center.localPosition);
    }
    void MouseFunc()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float angle = GetAngle();
            //Debug.Log(angle);
            if (angle > 45 && angle <= 135)
            {
                PRole.Jump(Direction.Right);
            }
            else if (angle > 0 && angle <= 45 || angle >315)
            {
                PRole.Jump(Direction.Top);
            }
            else if (angle > 225 && angle <= 315)
            {
                PRole.Jump(Direction.Left);
            }
            else
            {
                PRole.Jump(Direction.Bottom);
            }
        }
    }
}
