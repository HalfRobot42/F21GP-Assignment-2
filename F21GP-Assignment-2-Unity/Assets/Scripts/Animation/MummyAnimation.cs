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

    public GameObject Spell;

    public bool Walking; // fix
    public bool Grasp; // fix

    private bool previousGrasp; // store the value of Grasp in the previous update, needed to transition between idle animation and grasp animation

    private float swaySpeed = 2.5F;
    private bool swayDir = true; // keep track of swaying direction
    private float walkSpeed = 20F;
    private bool walkDir = true;

    private float swayRange = 5;
    private float walkRange = 7.5F;


    private void Start()
    {
        Spell.SetActive(false); // disable spell effect on start
    }


    void Update()
    {
        
        // 
        if (swayDir) // sway up
        {
            if (!Grasp) { LeftArm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); } // + z dir, only when not grasping
            RightArm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir

            if (!Grasp) { LeftForearm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); } // + z dir, only when not grasping
            RightForearm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir

            Body.transform.Rotate(-Time.deltaTime * swaySpeed,0,0); // - x dir
            Head.transform.Rotate(-Time.deltaTime * swaySpeed, 0, 0); // - x dir
        }
        else // flap down
        {
            if (!Grasp) { LeftArm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); } // - z dir, only when not grasping
            RightArm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed);   // + z dir

            if (!Grasp) { LeftForearm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); } // + z dir, only when not grasping
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


        if (Grasp)
        {
            if (!previousGrasp) // grasp was previously not active, set arm position to grasp angle
            {
                LeftArm.transform.localEulerAngles = new Vector3(0, 0, 80);
                LeftForearm.transform.localEulerAngles = new Vector3(0, 0, -80);

                Spell.SetActive(true); // enable spell effect
            }
        }
        else
        {
            if (previousGrasp) // was previously grasping, set arm position to idle
            {
                LeftArm.transform.localEulerAngles = new Vector3(0, 0, 0);
                LeftForearm.transform.localEulerAngles = new Vector3(0, 0, -90);

                Spell.SetActive(false); // disable spell effect
            }
        }


        previousGrasp = Grasp; // update previous Grasp to this updates Grasp
    }
}
