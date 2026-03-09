using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    
    void Update()
    {
        transform.position = cameraPosition.position; // constantly be moving camera to player position
        Debug.Log(transform.position);
    }
}
