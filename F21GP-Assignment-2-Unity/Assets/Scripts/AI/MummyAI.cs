using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MummyAI : MonoBehaviour
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

    public GameObject Skeleton;

    public GameObject Player; // player to chase

    public string areaName; // default area to wander around

    // enemy health
    public float health = 5;

    private bool walking = true; // true if walking
    private bool grasp = false; // true if grasping

    private bool previousGrasp = false; // store the value of Grasp in the previous update, needed to transition between idle animation and grasp animation

    private float swaySpeed = 2.5F;
    private bool swayDir = true; // keep track of swaying direction
    private float walkSpeed = 20F;
    private bool walkDir = true;

    // ranges for animations
    private float swayRange = 5;
    private float walkRange = 7.5F;

    // player vairables
    private float detectRadius = 10F;
    private float attackRadius = 5F;
    private bool playerDetected = false;


    // self components
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidBody;


    // variables for wandering (idle behaviour)
    private float wanderRadius = 2.5F;
    private float wanderTargetError = 0.5F;
    private int waitCount;
    private bool waiting = false;
    private Vector3 currentTarget;


    private void Awake()
    {
        // get self componenents
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        Spell.SetActive(false); // disable spell effect on start

        // set new random target position
        walking = true;
        currentTarget = FindRandomPosition();
        navMeshAgent.SetDestination(currentTarget);
    }


    void Update()
    {
        // if close enough to the player, start chasing them (measure from the head)
        if (Vector3.Distance(Head.transform.position, Player.transform.position) < detectRadius)
        {

            // find the vector pointing from self to the player
            Vector3 rayVector = Player.transform.position - Head.transform.position;

            // create ray to check we have a line of sight to the player
            RaycastHit rayQuery;
            Physics.Raycast(Head.transform.position, rayVector, out rayQuery, detectRadius);

            if (rayQuery.collider.tag == "Player")
            {
                playerDetected = true; // player is in line of sight
            }
        }

        if (playerDetected)
        {
            ChasePlayer(); // attack
        }
        else
        {
            Wander(); // idle behaviour
        }

        // run animations
        Animate();

        // check health and die if needed
        CheckHealth();
    }


    void CheckHealth()
    {
        if (health <= 0) // die now
        {
            // spawn a skeleton at our location
            GameObject RagDoll = Instantiate(Skeleton, transform.position, transform.rotation);

            // apply a force to scatter the pieces, apply to each child in the ragdoll
            foreach (Transform child in RagDoll.transform)
            {
                child.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * Random.Range(5F,10F), ForceMode.Force);
            }  

            // set inactive
            gameObject.SetActive(false);
        }
    }


    void ChasePlayer()
    {
        walking = true;

        // set target to current player position
        navMeshAgent.SetDestination(Player.transform.position);

        // if close enough to the player, attack
        if (Vector3.Distance(transform.position, Player.transform.position) < attackRadius)
        {
            grasp = true;
        }
        else
        {
            grasp = false;
        }

    }




    void Wander()
    {

        // find distance between self and current target
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget);

        // if close enough to target position, stop and start waiting
        if (distanceToTarget < wanderTargetError && !waiting)
        {
            // stop self
            navMeshAgent.ResetPath();
            rigidBody.velocity = new Vector3(0, 0, 0);
            rigidBody.angularVelocity = new Vector3(0, 0, 0);

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

        if (swayDir) // sway up
        {
            if (!grasp) { LeftArm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); } // + z dir, only when not grasping
            RightArm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir

            if (!grasp) { LeftForearm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); } // + z dir, only when not grasping
            RightForearm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); // - z dir

            Body.transform.Rotate(-Time.deltaTime * swaySpeed,0,0); // - x dir
            Head.transform.Rotate(-Time.deltaTime * swaySpeed, 0, 0); // - x dir
        }
        else // sway down
        {
            if (!grasp) { LeftArm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); } // - z dir, only when not grasping
            RightArm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed);   // + z dir

            if (!grasp) { LeftForearm.transform.Rotate(0, 0, -Time.deltaTime * swaySpeed); } // + z dir, only when not grasping
            RightForearm.transform.Rotate(0, 0, Time.deltaTime * swaySpeed); // - z dir

            Body.transform.Rotate(Time.deltaTime * swaySpeed, 0, 0); // + x dir
            Head.transform.Rotate(Time.deltaTime * swaySpeed, 0, 0); // + x dir
        }


        // deturmine direction to sway in (euler angles and loop around from 0-360)
        if (RightArm.transform.localEulerAngles.z > swayRange && RightArm.transform.localEulerAngles.z < 180) // if z angle is between range and 180, start sway "down"
        {
            swayDir = true;
        }
        else if (RightArm.transform.localEulerAngles.z < (360-swayRange) && RightArm.transform.localEulerAngles.z > 180) // if z angle is between -range and 180, start sway "up"
        {
            swayDir = false;
        }


        if (walking)
        {
            // swing legs back and forth
            if (walkDir)
            {
                RightLeg.transform.Rotate(0, 0, Time.deltaTime * walkSpeed);
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
            if (RightLeg.transform.localEulerAngles.z > walkRange && RightLeg.transform.localEulerAngles.z < 180) // if too high out of range, move "down"
            {
                walkDir = false; // this is arbitrery, but it remains consistent
            }
            else if (RightLeg.transform.localEulerAngles.z < (360 - walkRange) && RightLeg.transform.localEulerAngles.z > 180) // if too low out of range, move "up"
            {
                walkDir = true;
            }
        }
        else // do not rotate legs
        {
            // reset leg positions
            RightLeg.transform.localEulerAngles = new Vector3(0, -90, 0);
            RightCalf.transform.localEulerAngles = new Vector3(0, 0, -90);
            LeftLeg.transform.localEulerAngles = new Vector3(0, -90, 0);
            LeftCalf.transform.localEulerAngles = new Vector3(0, 0, -90);
        }


        if (grasp)
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


        previousGrasp = grasp; // update previous Grasp to this updates Grasp
    }
}
