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

    // gun animation
    public GameObject GunHolder; // gun to rotate (recoil) when shooting
    public GameObject BarrelPlacement; // gun barrel location to move spark effect too
    public ParticleSystem ParticleBarrel; // the spark effect to spawn on the gun barrel
    public ParticleSystem ParticleEnemy; // the spark effect to spawn on the enemy (1 enemy type per level, change depending on the level)
    public ParticleSystem ParticleClay; // the spark effect to spawn on clay items
    public ParticleSystem ParticleEnvironment; // the spark effect to spawn on the environment

    public Transform orientation; // player orientation


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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        if (Input.GetMouseButtonDown(0) && readyToShoot)
        {
            readyToShoot = false; // prevent continuous shooting
            StartCoroutine(ShootAnimation()); // start shoot animation coroutine
            Invoke(nameof(ResetShoot), shootCooldown); // don't wan the player shooting continuously, create cooldown to reset

            // cast a ray from the camera forward and see if we hit an enemy
            RaycastHit rayQuery;
            bool collision = Physics.Raycast(transform.position, transform.forward, out rayQuery, shootRange);

            // ray hit an object
            if (collision) {
                if (rayQuery.collider.tag == "Mummy") // hit an mummy
                {
                    // teleport the particle system to the collision point
                    ParticleEnemy.transform.position = rayQuery.point;
                    ParticleEnemy.Play(); // play spark animation

                    // reduce enemy health
                    rayQuery.transform.gameObject.GetComponent<MummyAI>().health--;
                }
                else if (rayQuery.collider.tag == "Lizard") // hit a lizard
                {
                    ParticleEnemy.transform.position = rayQuery.point;
                    ParticleEnemy.Play(); 

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

                    // add a force
                    rayQuery.transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 50F, ForceMode.Force);
                }
                else // hit a random object, probably a wall
                {
                    ParticleEnvironment.transform.position = rayQuery.point;
                    ParticleEnvironment.Play(); 
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
        while (GunHolder.transform.localEulerAngles.x > 10) 
        {
            GunHolder.transform.Rotate(Time.deltaTime * recoilSpeed,0,0); // + x dir
            yield return null;
        }
        
        // rotate back to original position
        GunHolder.transform.localEulerAngles = new Vector3(0, 0, 0);

    }

}