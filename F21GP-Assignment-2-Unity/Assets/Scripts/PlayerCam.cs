using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// code inspo
//https://www.youtube.com/watch?v=f473C43s8nE&t=128s
//https://www.youtube.com/watch?v=yl2Tv72tV7U

public class PlayerCam : MonoBehaviour
{
    // mouse sensitivity
    public float sensX;
    public float sensY;

    public Transform orientation; // player orientation

    //float xRotation;
    //float yRotation;


    // keep track of camera rotation
    private float cameraMoveY; 
    private float cameraMoveX;

    // for smooth mouse input 
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    private float mouseSmoothTime = 0.03f;



    private bool isObjectHeld = false;
    
    //public TMP_Text cameraDir;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // private void OnEnable()
    // {
    //     // subscribe to object pickup and drop events
    //     objectForce.OnObjectPickup += fixCameraRotation;
    //     objectForce.OnObjectDrop += resetCameraRotation;
    // }
    //
    // private void OnDisable()
    // {
    //     // unsubscribe if script is removed or destroyed
    //     objectForce.OnObjectPickup -= fixCameraRotation;
    //     objectForce.OnObjectDrop -= resetCameraRotation;
    // }

    private void Update()
    {
        // get input vector
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // move current mouse position towards target
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        // move orientation by this amount, keeping track of past vaues by adding. also apply sensitivity values
        cameraMoveY -= currentMouseDelta.y * sensY;
        cameraMoveX += currentMouseDelta.x * sensX;

        // cap y look depending on if holding an object or not
        if (isObjectHeld)
        {
            cameraMoveY = Mathf.Clamp(cameraMoveY, -80f, 35f);
        }
        else
        {
            cameraMoveY = Mathf.Clamp(cameraMoveY, -90.0f, 90.0f);
        }

        // apply rotations
        transform.rotation = Quaternion.Euler(cameraMoveY, cameraMoveX, 0); // rotate the camera
        orientation.rotation = Quaternion.Euler(0, cameraMoveX, 0); // we only want to rotate the player horizontally (around the y axis)


        // old code
        /*
        // mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        if (isObjectHeld)
        {
           xRotation = Mathf.Clamp(xRotation, -80f, 35f); 
        }
        else
        {
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        }
        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        // cameraDir.text = "Camera Dir: " + orientation.position;
        */
    }
}
