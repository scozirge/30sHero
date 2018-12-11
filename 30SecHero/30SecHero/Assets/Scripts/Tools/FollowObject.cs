using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    GameObject Target;
    [SerializeField]
    float LerpFactor;
    Vector3 Offset;
    Vector3 OriginalPos;
    public bool FixX = false;
    public bool FixY = false;
    public bool FixZ = true;
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        OriginalPos = transform.position;
        Offset = transform.position - Target.transform.position;
    }
    void FixedUpdate()
    {
        if (!Target)
            return;
        Vector3 destination = Target.transform.position + Offset;
        if (FixX)
            destination.x = OriginalPos.x;
        if (FixY)
            destination.y = OriginalPos.y;
        if (FixZ)
            destination.z = OriginalPos.z;
        transform.position = Vector3.Lerp(transform.position, destination, LerpFactor);

    }
}
