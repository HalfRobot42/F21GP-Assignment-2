using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimation : MonoBehaviour
{

    // wings and body
    public GameObject WingLeft;
    public GameObject WingRight;
    public GameObject Body;

    public float batHeight;

    private float flapSpeed = 1000F; 
    private float bobSpeed = 0.5F; // how fast the bat "bobs" up and down
    private float bobError = 0.5F; // make sure the bat doesn't bob out of range
    private bool flapDir = true; // keep track of flapping direction (true is up)

    private float flapTopRange = 80;
    private float flapBottomRange = 300;

    private void Start()
    {
        Body.transform.position = Body.transform.position + new Vector3(0, batHeight, 0); // set bat at required height
    }

    void Update()
    {
        // flap wings, bob
        if (flapDir) // flap up, bob downward
        {
            WingRight.transform.Rotate(0, 0, Time.deltaTime * flapSpeed); // + z dir
            WingLeft.transform.Rotate(0, 0, -Time.deltaTime * flapSpeed); // - z dir

            // bob downward
            Body.transform.position = Body.transform.position + new Vector3(0, -bobSpeed * Time.deltaTime, 0);

            // make sure we don't bob out of bounds, cap height
            if (Body.transform.position.y < (batHeight - bobError))
            {
                Body.transform.position = new Vector3(Body.transform.position.x, batHeight - bobError, Body.transform.position.z);
            }
        }
        else // flap down, bob upward
        {
            WingRight.transform.Rotate(0, 0, -Time.deltaTime * flapSpeed); // - z dir
            WingLeft.transform.Rotate(0, 0, Time.deltaTime * flapSpeed);   // + z dir

            // bob upward
            Body.transform.position = Body.transform.position + new Vector3(0, bobSpeed * Time.deltaTime, 0);

            // make sure we don't bob out of bounds, cap height
            if (Body.transform.position.y > (batHeight + bobError))
            {
                Body.transform.position = new Vector3(Body.transform.position.x, batHeight + bobError, Body.transform.position.z);
            }
        }


        // deturmine direction to flap in (euler angles and loop around from 0-360)
        if (WingRight.transform.eulerAngles.z > flapTopRange && WingRight.transform.eulerAngles.z < 180) // if z angle is between range and 180, start flap down
        {
            flapDir = false;
        }
        else if (WingRight.transform.eulerAngles.z < flapBottomRange && WingRight.transform.eulerAngles.z > 180) // if z angle is between -range and 180, start flap up
        {
            flapDir = true;
        }


    }       

}
