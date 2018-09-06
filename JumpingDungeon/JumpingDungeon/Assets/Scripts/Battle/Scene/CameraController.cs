using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Vector2 ScreenSize;
    [SerializeField]
    GameObject Player;       //Public variable to store a reference to the player game object
    Vector3 Offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        Offset = transform.position - Player.transform.position;
    }
    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (!Player)
            return;
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x, 0, 0) + Offset, 0.05f);
    }

}
