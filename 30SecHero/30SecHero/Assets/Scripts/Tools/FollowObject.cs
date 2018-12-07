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
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        Offset = transform.position - Target.transform.position;
    }
    void FixedUpdate()
    {
        if (!Target)
            return;
        transform.position = Vector3.Lerp(transform.position, new Vector3(Target.transform.position.x, 0, 0) + Offset, LerpFactor);

    }
}
