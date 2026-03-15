using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatAI : MonoBehaviour
{

    // wings and body
    public GameObject WingLeft;
    public GameObject WingRight;
    public GameObject Body;

    public GameObject Player; // player target
    public List<Transform> lightList; // list of all other light sources in the level

    private float batHeightTopRange = 2.5F;
    private float batHeightBottomRange = 1.5F;
    private float batHeight = 2;

    // the radius at which to avoid the lights and player
    private float lightRadius = 2.5F;
    private float playerRadius = 2.5F;

    private float flapSpeed = 1000F; 
    private float bobSpeed = 0.5F; // how fast the bat "bobs" up and down
    private float bobError = 0.5F; // make sure the bat doesn't bob out of range
    private bool flapDir = true; // keep track of flapping direction (true is up)

    private float flapTopRange = 80;
    private float flapBottomRange = 300;


    // self components
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidBody;

    private void Awake()
    {
        // get self componenents
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        RandomStartAnimation();
    }

    private void Update()
    {
        ChasePlayer();
        Animate();
    }

    void ChasePlayer()
    {
        // set target to current player position
        navMeshAgent.SetDestination(Player.transform.position);

        // remove y components (bats need only track on the horizontal plane)
        Vector3 selfPlanePosition = new Vector3(transform.position.x,0,transform.position.z);
        Vector3 playerPlanePosition = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);

        // if within the light radius around the player, apply a force
        if (Vector3.Distance(selfPlanePosition, playerPlanePosition) < playerRadius)
        {
            // find the vector pointing from the player to self
            Vector3 forceVector = selfPlanePosition - playerPlanePosition;

            // apply force to rigidbody
            rigidBody.AddForce(forceVector * 0.2F, ForceMode.Force);
        }

        // iterate through the other light sources in the level
        for (int i = 0; i < lightList.Count; i++)
        {
            Vector3 lightPlanePosition = new Vector3(lightList[i].transform.position.x, 0, lightList[i].transform.position.z);

            // if within the set radius around this light, apply a force
            if (Vector3.Distance(selfPlanePosition, lightPlanePosition) < lightRadius)
            {
                // find the vector pointing from the light to self
                Vector3 forceVector = selfPlanePosition - lightPlanePosition;

                // apply force to rigidbody
                rigidBody.AddForce(forceVector * 0.2F, ForceMode.Force);
            }
        }
    }


    void RandomStartAnimation()
    {
        float randomWingAngle = Random.Range(flapBottomRange, flapTopRange);
        batHeight = Random.Range(batHeightBottomRange, batHeightTopRange);

        WingRight.transform.localEulerAngles = new Vector3(0, 0, randomWingAngle);
        WingLeft.transform.localEulerAngles = new Vector3(0, 0, -randomWingAngle);

        Body.transform.position = Body.transform.position + new Vector3(0, batHeight, 0); // set bat at required height
    }

    void Animate()
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
        if (WingRight.transform.localEulerAngles.z > flapTopRange && WingRight.transform.localEulerAngles.z < 180) // if z angle is between range and 180, start flap down
        {
            flapDir = false;
        }
        else if (WingRight.transform.localEulerAngles.z < flapBottomRange && WingRight.transform.localEulerAngles.z > 180) // if z angle is between -range and 180, start flap up
        {
            flapDir = true;
        }


    }       

}
