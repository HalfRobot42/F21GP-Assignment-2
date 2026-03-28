using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// code inspo (for basic player setup)
//https://www.youtube.com/watch?v=f473C43s8nE&t=128s
//https://www.youtube.com/watch?v=yl2Tv72tV7U

public class CameraControl : MonoBehaviour
{
    // mouse sensitivity
    public float sensX;
    public float sensY;

    public bool holdingGun = true; // switch between gun and torch
    private const float lightLevelMax = 10;
    public float lightLevel = lightLevelMax; // will decrease over time, if it runs out the bats may attack
    public List<Transform> lightList; // list of all light sources in the level to relight the torch
    private float lightRadius = 2.2F; // area around the light where we can relight the torch

    // gun animation
    public GameObject GunHolder; // gun to rotate (recoil) when shooting
    public GameObject TorchHolder; // object with torch as child
    public GameObject TorchLight; // object attatched to player to toggle
    public GameObject BarrelPlacement; // gun barrel location to move spark effect too
    public ParticleSystem ParticleBarrel; // the spark effect to spawn on the gun barrel
    public ParticleSystem ParticleEnemy; // the spark effect to spawn on the enemy (1 enemy type per level, change depending on the level)
    public ParticleSystem ParticleClay; // the spark effect to spawn on clay items
    public ParticleSystem ParticleEnvironment; // the spark effect to spawn on the environment

    // sound effects
    public AudioClip gunShotSound;
    public AudioClip enemyShotSound;
    public AudioClip clayPotSound;

    // environment destruction
    public GameObject PotRagdoll;
    public GameObject PotLargeRagdoll;

    // player orientation
    public Transform orientation; 


    // keep track of camera rotation
    private float cameraMoveY; 
    private float cameraMoveX;

    // for smooth mouse input 
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    private float mouseSmoothTime = 0.03f;

    // shooting variables
    private bool readyToShoot = true;
    private float shootCooldown = 0.3F;
    private float shootRange = 20F;

    // gun animation variables
    private float recoilSpeed = 500F;
    private float recoilAngle = 60;

    private void Start()
    {
        // set global volume to max
        AudioListener.volume = 1.0F;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if(holdingGun) // player is in gun mode, show gun
        {
            GunHolder.SetActive(true);
            // hide torch and light
            TorchHolder.SetActive(false);
            TorchLight.SetActive(false);
        }
        else
        {
            GunHolder.SetActive(false);
            //TorchHolder.SetActive(true);
            //TorchLight.SetActive(true);
        }
    }
    

    private void Update()
    {
        // get input vector
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // move current mouse position towards target
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        // move orientation by this amount, keeping track of past vaues by adding. also apply sensitivity values
        cameraMoveY -= currentMouseDelta.y * sensY;
        cameraMoveX += currentMouseDelta.x * sensX;

        
        cameraMoveY = Mathf.Clamp(cameraMoveY, -90.0f, 90.0f);

        // apply rotations
        transform.rotation = Quaternion.Euler(cameraMoveY, cameraMoveX, 0); // rotate the camera
        orientation.rotation = Quaternion.Euler(0, cameraMoveX, 0); // we only want to rotate the player horizontally (around the y axis)


        // get left click input
        if (Input.GetMouseButtonDown(0) && readyToShoot && holdingGun)
        {
            readyToShoot = false; // prevent continuous shooting
            StartCoroutine(ShootAnimation()); // start shoot animation coroutine
            Invoke(nameof(ResetShoot), shootCooldown); // don't want the player shooting continuously, create cooldown to reset

            // cast a ray from the camera forward and see if we hit an enemy
            RaycastHit rayQuery;
            bool collision = Physics.Raycast(transform.position, transform.forward, out rayQuery, shootRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

            // ray hit an object
            if (collision) {
                if (rayQuery.collider.tag == "Mummy") // hit an mummy
                {
                    // teleport the particle system to the collision point
                    ParticleEnemy.transform.position = rayQuery.point;
                    ParticleEnemy.Play(); // play spark animation

                    AudioSource.PlayClipAtPoint(enemyShotSound, rayQuery.point, 1.0F); // play enemy sound effect

                    // reduce enemy health
                    rayQuery.transform.gameObject.GetComponent<MummyAI>().health--;
                }
                else if (rayQuery.collider.tag == "Lizard") // hit a lizard
                {
                    ParticleEnemy.transform.position = rayQuery.point;
                    ParticleEnemy.Play();

                    AudioSource.PlayClipAtPoint(enemyShotSound, rayQuery.point, 1.0F); // play enemy sound effect

                    // reduce enemy health
                    rayQuery.transform.gameObject.GetComponent<LizardAI>().health--;
                }
                else if (rayQuery.collider.tag == "LizardDead") // hit a dead lizard
                {
                    ParticleEnemy.transform.position = rayQuery.point;
                    ParticleEnemy.Play();
                }
                else if(rayQuery.collider.tag == "Clay") // hit a clay item
                {
                    ParticleClay.transform.position = rayQuery.point;
                    ParticleClay.Play(); 
                }
                else if (rayQuery.collider.tag == "ClayPot") // hit a clay pot
                {
                    ParticleClay.transform.position = rayQuery.point;
                    ParticleClay.Play();

                    AudioSource.PlayClipAtPoint(clayPotSound, rayQuery.point, 1.0F); // play clay pot breaking sound effect

                    // replace the pot with a ragdoll
                    // spawn a ragdoll pot at the pots location
                    GameObject RagDoll = Instantiate(PotRagdoll, rayQuery.collider.transform.position, rayQuery.collider.transform.rotation);

                    // apply a random force to scatter the pieces, apply to each child in the ragdoll
                    foreach (Transform child in RagDoll.transform)
                    {
                        // create a random direction vector
                        Vector3 forceDir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                        forceDir = forceDir.normalized;

                        float forceMag = Random.Range(5F, 10F);

                        child.gameObject.GetComponent<Rigidbody>().AddForce(forceDir * forceMag, ForceMode.Force);
                    }

                    // set original pot inactive
                    rayQuery.collider.gameObject.SetActive(false);
                }
                else if (rayQuery.collider.tag == "ClayPotLarge") // hit a large clay pot
                {
                    ParticleClay.transform.position = rayQuery.point;
                    ParticleClay.Play();

                    AudioSource.PlayClipAtPoint(clayPotSound, rayQuery.point, 1.0F); // play clay pot breaking sound effect

                    // replace the pot with a ragdoll
                    // spawn a ragdoll pot at the pots location
                    GameObject RagDoll = Instantiate(PotLargeRagdoll, rayQuery.collider.transform.position, rayQuery.collider.transform.rotation);

                    // apply a random force to scatter the pieces, apply to each child in the ragdoll
                    foreach (Transform child in RagDoll.transform)
                    {
                        // create a random direction vector
                        Vector3 forceDir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
                        forceDir = forceDir.normalized;

                        float forceMag = Random.Range(5F, 10F);

                        child.gameObject.GetComponent<Rigidbody>().AddForce(forceDir * forceMag, ForceMode.Force);
                    }

                    // set original pot inactive
                    rayQuery.collider.gameObject.SetActive(false);
                }
                else // hit a random object, probably a wall
                {
                    ParticleEnvironment.transform.position = rayQuery.point;
                    ParticleEnvironment.Play(); 
                }
            }
            
        }


        if (!holdingGun) // player is in torch mode
        {
            Debug.Log(lightLevel);

            if (lightLevel > 0) // if light level is above 0, show the torch
            {
                TorchHolder.SetActive(true);
                TorchLight.SetActive(true);
            }
            else // below zero hide the torch
            {
                TorchHolder.SetActive(false); 
                TorchLight.SetActive(false);
            }


            //Debug.Log(Vector3.Distance(transform.position, lightList[1].transform.position));

            // iterate through the light sources in the level
            bool refill = false;
            for (int i = 0; i < lightList.Count; i++)
            {

                // if within the set radius around this light, 
                if (Vector3.Distance(transform.position, lightList[i].transform.position) < lightRadius)
                {
                    refill = true;
                    lightLevel = lightLevelMax;
                }
            }

            if (!refill) // if we didn't refill the light level, reduce it
            {
                if (lightLevel > 0) // only reduce if above 0
                {
                    lightLevel -= 1F * Time.deltaTime;
                }
                
            }
        }


        // teleport the barrel effect to the barrel of the gun
        ParticleBarrel.transform.position = BarrelPlacement.transform.position;
    }


    private void ResetShoot()
    {
        readyToShoot = true;
    }


    IEnumerator ShootAnimation()
    {
        // play shoot sound
        AudioSource.PlayClipAtPoint(gunShotSound, BarrelPlacement.transform.position); 

        // teleport the particle effect to the barrel of the gun
        ParticleBarrel.transform.position = BarrelPlacement.transform.position;
        ParticleBarrel.Play(); // play spark animation

        // reduce angle so now (because of euler) angles are below 360
        GunHolder.transform.localEulerAngles = new Vector3(359.9F, 0, 0); 

        // while angle has not reached the recoil angle desired (below 360)
        while (GunHolder.transform.localEulerAngles.x > (360-recoilAngle)) { 
            GunHolder.transform.Rotate(-Time.deltaTime * recoilSpeed,0,0); // - x dir
            yield return null;
        }

        // need to rotate upwards until the euler angles wraps round to 0 again, can check this by seeing if the angle is greater than a low number
        // will stop when the angle is now between 0 and this low number
        while (GunHolder.transform.localEulerAngles.x > 40) 
        {
            GunHolder.transform.Rotate(Time.deltaTime * recoilSpeed,0,0); // + x dir
            yield return null;
        }
        
        // rotate back to original position
        GunHolder.transform.localEulerAngles = new Vector3(0, 0, 0);

    }

}