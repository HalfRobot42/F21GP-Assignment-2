using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardAI : MonoBehaviour
{

    // wings and body
    public GameObject TailSegment1Holder;
    public GameObject TailSegment2Holder;
    public GameObject LegBackRight;
    public GameObject LegBackLeft;
    public GameObject LegFrontRight;
    public GameObject LegFrontLeft;
    public GameObject TopJaw;
    public GameObject BottomJaw;
    public bool Walking; // fix this later
    public bool Bite; // also fix


    private bool previousBite; // store the value of bite in the previous update, needed to transition between idle animation and bite animation

    private float waveSpeed = 25F; // speed at which to wave the tail
    private bool waveDir = true; // keep track of waving direction (direction is arbitrary, but consistent)
    private float walkSpeed = 35F;
    private bool walkDir = true;
    private float jawSpeed = 5F; // jaw idle animation 
    private bool jawDir = true;

    // jaw open angles
    private float biteTopAngle = -20F;
    private float biteBottomAngle = 20F;

    // angle ranges in which body parts "swing" within
    private float waveRange = 30F;
    private float jawRange = 2.5F;
    private float walkRange = 10F;


    void Update()
    {
        // wave tail
        if (waveDir) 
        {
            TailSegment1Holder.transform.Rotate(0, Time.deltaTime * waveSpeed, 0);  // + y
            TailSegment2Holder.transform.Rotate(0, Time.deltaTime * waveSpeed, 0); 

        }
        else 
        {
            TailSegment1Holder.transform.Rotate(0, -Time.deltaTime * waveSpeed, 0); // - y
            TailSegment2Holder.transform.Rotate(0, -Time.deltaTime * waveSpeed, 0);  
        }


        // deturmine direction to wave in (euler angles and loop around from 0-360)
        if (TailSegment1Holder.transform.eulerAngles.y > waveRange && TailSegment1Holder.transform.eulerAngles.y < 180) // if greater then open angle + range, move "down"
        {
            waveDir = false;
        }
        else if (TailSegment1Holder.transform.eulerAngles.y < (360-waveRange) && TailSegment1Holder.transform.eulerAngles.y > 180) // if less than open angle - range, move "up"
        {
            waveDir = true;
        }



        if (Bite) // "bite" means having the jaw open, used when near the player
        {
            if (!previousBite) // if the jaw was closed before, need to open the jaw
            {
                // open jaw
                TopJaw.transform.localEulerAngles = new Vector3(biteTopAngle, 180, 0);
                BottomJaw.transform.localEulerAngles = new Vector3(biteBottomAngle, 180, 0);
            }
            

            // perform jaw idle animation
            if (jawDir)
            {
                TopJaw.transform.Rotate(Time.deltaTime * jawSpeed, 0, 0);
                BottomJaw.transform.Rotate(-Time.deltaTime * jawSpeed, 0, 0);

            }
            else
            {
                TopJaw.transform.Rotate(-Time.deltaTime * jawSpeed, 0, 0);
                BottomJaw.transform.Rotate(Time.deltaTime * jawSpeed, 0, 0);
            }


            // determine direction to move jaws in (these eueler angles are all positive)
            if ( BottomJaw.transform.eulerAngles.x > (jawRange+biteBottomAngle)) // if greater then open angle + range, move "down"
            {
                jawDir = true;
            }
            else if (BottomJaw.transform.eulerAngles.x < (biteBottomAngle-jawRange) ) // if less than open angle - range, move "up"
            {
                jawDir = false;
            }
        }
        else // no bite
        {   
            if (previousBite) // jaw was previously open, need to close
            {
                // close jaw
                TopJaw.transform.localEulerAngles = new Vector3(0, 180, 0);
                BottomJaw.transform.localEulerAngles = new Vector3(5, 180, 0);
            }
            

            // jaw idle animation
            if (jawDir)
            {
                TopJaw.transform.Rotate(Time.deltaTime * jawSpeed, 0, 0);
                BottomJaw.transform.Rotate(-Time.deltaTime * jawSpeed, 0, 0);
            }
            else
            {
                TopJaw.transform.Rotate(-Time.deltaTime * jawSpeed, 0, 0);
                BottomJaw.transform.Rotate(Time.deltaTime * jawSpeed, 0, 0);
            }


            // deturmine direction to move jaw in (euler angles and loop around from 0-360)
            if (TopJaw.transform.eulerAngles.x > jawRange && TopJaw.transform.eulerAngles.x < 180) // if too high out of range, move "down"
            {
                jawDir = false;
            }
            else if (TopJaw.transform.eulerAngles.x < (360-jawRange) && TopJaw.transform.eulerAngles.x > 180) // if too low out of range, move "up"
            {
                jawDir = true;
            }
        }



        if (Walking) // use walking animation
        { 

            // swing legs back and forth
            if (walkDir)
            {
                LegBackRight.transform.Rotate(Time.deltaTime * walkSpeed, 0, 0);
                LegBackLeft.transform.Rotate(Time.deltaTime * walkSpeed, 0, 0);
                LegFrontRight.transform.Rotate(-Time.deltaTime * walkSpeed, 0, 0);
                LegFrontLeft.transform.Rotate(-Time.deltaTime * walkSpeed, 0, 0);
            }
            else
            {
                LegBackRight.transform.Rotate(-Time.deltaTime * walkSpeed, 0, 0);
                LegBackLeft.transform.Rotate(-Time.deltaTime * walkSpeed, 0, 0);
                LegFrontRight.transform.Rotate(Time.deltaTime * walkSpeed, 0, 0);
                LegFrontLeft.transform.Rotate(Time.deltaTime * walkSpeed, 0, 0);
            }

            // determine "direction" to walk in
            if (LegBackRight.transform.eulerAngles.x > walkRange && LegBackRight.transform.eulerAngles.x < 180) // if too high out of range, move "down"
            {
                walkDir = true; // this is arbitrery, but it remains consistent
            }
            else if (LegBackRight.transform.eulerAngles.x < (360-walkRange) && LegBackRight.transform.eulerAngles.x > 180) // if too low out of range, move "up"
            {
                walkDir = false;
            }
        }
        else // do not rotate legs
        {
            // note that this does not reset their position
            LegBackRight.transform.Rotate(0, 0, 0);
            LegBackLeft.transform.Rotate(0, 0, 0);
            LegFrontRight.transform.Rotate(0, 0, 0);
            LegFrontLeft.transform.Rotate(0, 0, 0);
        }


        previousBite = Bite; // update previous bite to this updates bite

    }
}
