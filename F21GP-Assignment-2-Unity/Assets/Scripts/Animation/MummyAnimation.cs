using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyAnimation : MonoBehaviour
{



    // body parts
    public GameObject Body;
    public GameObject Head;

    public GameObject LeftArm;
    public GameObject LeftForearm;
    public GameObject RightArm;
    public GameObject RightForearm;

    public GameObject LeftLeg;
    public GameObject LeftCalf;
    public GameObject RightLeg;
    public GameObject RightCalf;

    public bool Walking; // fix
    public bool Attack; // fix


    private float swaySpeed = 2.5F;
    private bool swayDir = true; // keep track of swaying direction
    private float walkSpeed = 20F;
    private bool walkDir = true;

    private float swayRange = 5;
    private float walkRange = 7.5F;


    void Update()
    {
        
        // 
        if (swayDir) // sway up, bob downward
        {
            LeftArm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); // + z dir
            RightArm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir

            LeftForearm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); // + z dir
            RightForearm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir

            Body.transform.Rotate(-Time.deltaTime * swaySpeed,0,0); // - x dir
            Head.transform.Rotate(-Time.deltaTime * swaySpeed, 0, 0); // - x dir
        }
        else // flap down, bob upward
        {
            LeftArm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir
            RightArm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed);   // + z dir

            LeftForearm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // + z dir
            RightForearm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); // - z dir

            Body.transform.Rotate(Time.deltaTime * swaySpeed, 0, 0); // + x dir
            Head.transform.Rotate(Time.deltaTime * swaySpeed, 0, 0); // + x dir
        }


        // deturmine direction to sway in (euler angles and loop around from 0-360)
        if (RightArm.transform.eulerAngles.z > swayRange && RightArm.transform.eulerAngles.z < 180) // if z angle is between range and 180, start sway "down"
        {
            swayDir = true;
        }
        else if (RightArm.transform.eulerAngles.z < (360-swayRange) && RightArm.transform.eulerAngles.z > 180) // if z angle is between -range and 180, start sway "up"
        {
            swayDir = false;
        }


        if (Walking)
        {
            // swing legs back and forth
            if (walkDir)
            {
                RightLeg.transform.Rotate(0,0,Time.deltaTime * walkSpeed);
                RightCalf.transform.Rotate(0, 0, Time.deltaTime * walkSpeed);
                LeftLeg.transform.Rotate(0, 0, -Time.deltaTime * walkSpeed);
                LeftCalf.transform.Rotate(0, 0, -Time.deltaTime * walkSpeed);
            }
            else
            {
                RightLeg.transform.Rotate(0, 0, -Time.deltaTime * walkSpeed);
                RightCalf.transform.Rotate(0, 0, -Time.deltaTime * walkSpeed);
                LeftLeg.transform.Rotate(0, 0, Time.deltaTime * walkSpeed);
                LeftCalf.transform.Rotate(0, 0, Time.deltaTime * walkSpeed);
            }

            // determine "direction" to walk in
            if (RightLeg.transform.eulerAngles.z > walkRange && RightLeg.transform.eulerAngles.z < 180) // if too high out of range, move "down"
            {
                walkDir = false; // this is arbitrery, but it remains consistent
            }
            else if (RightLeg.transform.eulerAngles.z < (360 - walkRange) && RightLeg.transform.eulerAngles.z > 180) // if too low out of range, move "up"
            {
                walkDir = true;
            }
        }
        else // do not rotate legs
        {
            // reset leg positions
            RightLeg.transform.localEulerAngles = new Vector3(0, 0, 0);
            RightCalf.transform.localEulerAngles = new Vector3(0, 0, -90);
            LeftLeg.transform.localEulerAngles = new Vector3(0, 0, 0);
            LeftCalf.transform.localEulerAngles = new Vector3(0, 0, -90);
        }


    }
}
