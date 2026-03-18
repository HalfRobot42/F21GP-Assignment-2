using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public GameObject Player;

    public string areaName; // default area to wander around

    // enemy health
    public float health = 5;

    // stores if certain animations are active
    private bool walking = false; 
    private bool bite = false; 

    private bool previousBite = false; // store the value of bite in the previous update, needed to transition between idle animation and bite animation

    private float waveSpeed = 25F; // speed at which to wave the tail
    private bool waveDir = true; // keep track of waving direction (direction is arbitrary, but consistent)

    private const float walkSpeedHigh = 150F; // speed to wave legs in when running
    private const float walkSpeedLow = 35F; // speed to wave legs in when walking idle
    private float walkSpeed = walkSpeedLow;
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


    // self components
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidBody;


    // variables for wandering (idle behaviour)
    private float wanderRadius = 10F;//2.5F;
    private float wanderTargetError = 0.5F;
    private int waitCount;
    private bool waiting = false;
    private Vector3 currentTarget;


    // player chase variables
    private bool playerDetected = false;
    private bool playerDetectedPrevious = false;
    private int playerDetectedCount = 0;
    private float detectRadius = 20F;
    private float targetError = 1F;//0.2F;

    // speed variables
    private const float speedHigh = 2F; // when chasing the player
    private const float speedLow = 0.5F; // idle walking speed

    private void Awake()
    {
        // get self componenents
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        navMeshAgent.speed = speedLow; // walking speed

        // set new random target position
        walking = true;
        currentTarget = FindRandomPosition();
        navMeshAgent.SetDestination(currentTarget);
    }


    private void Update()
    {

        // check if player is close enough and there is a straight line
        if (Vector3.Distance(transform.position, Player.transform.position) < detectRadius)
        {
            // find the vector pointing from self to the player
            Vector3 rayVector = Player.transform.position - transform.position;

            RaycastHit rayQuery;
            Physics.Raycast(transform.position, rayVector, out rayQuery, detectRadius);

            if (rayQuery.collider.tag == "Player")
            {
                playerDetected = true; // player is in line of sight
            }
            else
            {
                playerDetected = false; // player is not in line of sight
            }
        }

        if (playerDetected)
        {
            if (!playerDetectedPrevious) // was previously wandering
            {
                playerDetectedCount++; // keep track of number of time we've seen the player

                // start walking animation
                walking = true;

                // start running
                walkSpeed = walkSpeedHigh;
                navMeshAgent.speed = speedHigh;

                // open jaw
                bite = true;

                // set target to current player position
                navMeshAgent.SetDestination(Player.transform.position);
            }
            chasePlayer();
        }
        else
        {
            if (playerDetectedPrevious) // was previously chasing
            {
                // stop and wait
                Stop();
                walking = false;

                // close jaw
                bite = false;

                // set walking animation vars again
                walkSpeed = walkSpeedLow;
                navMeshAgent.speed = speedLow;
            }

            if (playerDetectedCount == 0)
            {
                Wander(); // idle actions, if we have not yet seen the player
            }
            

        }
            
        Animate();

        playerDetectedPrevious = playerDetected; // update previous detected to current for next update
    }


    void chasePlayer()
    {
        // when this function is called, target has already been set, need to monitor it
        if (Vector3.Distance(transform.position, navMeshAgent.destination) < targetError) // check how close the agent is to it's current target (players previous positon)
        {
            // agent is close enough

            // stop and reset the player detect
            Stop();
            walking = false;
            playerDetected = false;
        }
    }


    void Stop()
    {
        // stop self
        navMeshAgent.ResetPath();
        rigidBody.velocity = new Vector3(0, 0, 0);
        rigidBody.angularVelocity = new Vector3(0, 0, 0);
    }


    void Wander()
    {

        // find distance between self and current target
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget);

        // if close enough to target position, stop and start waiting
        if (distanceToTarget < wanderTargetError && !waiting)
        {
            // stop self
            Stop();

            walking = false;

            // create a random amount of time to wait
            waitCount = Random.Range(500, 1000);

            waiting = true;
        }

        // wait, and then find new random target
        if (waiting)
        {
            if (waitCount > 0) // delay
            {
                waitCount--;
            }
            else // set new random target position, and stop waiting
            {
                walking = true;
                currentTarget = FindRandomPosition();
                navMeshAgent.SetDestination(currentTarget);

                waiting = false;
            }
        }

    }



    Vector3 FindRandomPosition()
    {
        // create a random point in a sphere around self
        Vector3 randomPoint = Random.insideUnitSphere * wanderRadius;
        randomPoint += transform.position;

        // get the closest postion on the set area navmesh to this point
        NavMeshHit queryReturn;
        NavMesh.SamplePosition(randomPoint, out queryReturn, wanderRadius, 1 << NavMesh.GetAreaFromName(areaName));
        Vector3 finalPosition = queryReturn.position;

        return finalPosition;
    }



    void Animate()
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
        if (TailSegment1Holder.transform.localEulerAngles.y > waveRange && TailSegment1Holder.transform.localEulerAngles.y < 180) // if greater then open angle + range, move "down"
        {
            waveDir = false;
        }
        else if (TailSegment1Holder.transform.localEulerAngles.y < (360-waveRange) && TailSegment1Holder.transform.localEulerAngles.y > 180) // if less than open angle - range, move "up"
        {
            waveDir = true;
        }



        if (bite) // "bite" means having the jaw open, used when near the player
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
            if ( BottomJaw.transform.localEulerAngles.x > (jawRange+biteBottomAngle)) // if greater then open angle + range, move "down"
            {
                jawDir = true;
            }
            else if (BottomJaw.transform.localEulerAngles.x < (biteBottomAngle-jawRange) ) // if less than open angle - range, move "up"
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
            if (TopJaw.transform.localEulerAngles.x > jawRange && TopJaw.transform.localEulerAngles.x < 180) // if too high out of range, move "down"
            {
                jawDir = false;
            }
            else if (TopJaw.transform.localEulerAngles.x < (360-jawRange) && TopJaw.transform.localEulerAngles.x > 180) // if too low out of range, move "up"
            {
                jawDir = true;
            }
        }



        if (walking) // use walking animation
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
            if (LegBackRight.transform.localEulerAngles.x > walkRange && LegBackRight.transform.localEulerAngles.x < 180) // if too high out of range, move "down"
            {
                walkDir = true; // this is arbitrery, but it remains consistent
            }
            else if (LegBackRight.transform.localEulerAngles.x < (360-walkRange) && LegBackRight.transform.localEulerAngles.x > 180) // if too low out of range, move "up"
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


        previousBite = bite; // update previous bite to this updates bite

    }
}
