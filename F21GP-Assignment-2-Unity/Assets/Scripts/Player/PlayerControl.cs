using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// code inspo
//https://www.youtube.com/watch?v=f473C43s8nE&t=128s

public class PlayerControl : MonoBehaviour
{

    public AudioClip walkingGrassSound;
    public AudioClip walkingStoneSound;
    public AudioClip walkingSandSound;
    public AudioSource playerAudioSource;
    private bool readyToPlayWalk = true;
    private float walkSoundCooldown = 0.4F;


    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    private KeyCode jumpKey = KeyCode.Space;
    

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    

    [Header("Orientation")]

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    [Header("Health")]

    // player health
    public int health = 5; // actual health value to reduce
    private int maxHealth; // starting health
    public TMP_Text healthText; // text ui to update

    Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.freezeRotation = true; // stop player from falling over

        readyToJump = true;

        //Cursor.lockState = CursorLockMode.Locked; // Lock cursor to screen

        // set maxhealth to starting health value
        maxHealth = health;
    }

    private void Update()
    {
        //update health ui
        healthText.text = "Health: " + health;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        KeyboardInput();
        SpeedControl();

        WalkingSound();

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0; // no drag in the air
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void KeyboardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D
        verticalInput = Input.GetAxisRaw("Vertical"); // W/S

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown); // reset jump after x seconds, can jump again
        }
    }

    private void MovePlayer()
    {
        // calc move direction
        //moveDirection =  verticalInput * orientation.forward * Time.deltaTime + orientation.right * horizontalInput * Time.deltaTime;
        moveDirection = verticalInput * orientation.forward + orientation.right * horizontalInput; // multiply wasd inputs by the appropriate vectors


        //moveDirection.y = 0;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); // move in the calculated direction
        }

        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); // move faster in the air
        }
        
    }


    private void SpeedControl()
    {

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // get horizontal velocity
        

        if (flatVel.magnitude > moveSpeed) // cap player speed by move speed
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); // keep original y speed, otherwise can pause the player mid air
        }
    }  

    private void Jump()
    {
        // reset y vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // impulse force into the air
    }

    private void ResetJump()
    {
        readyToJump = true;
    }


    private void WalkingSound()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // get horizontal velocity

        if (grounded && (horizontalInput != 0 || verticalInput != 0) && (flatVel.magnitude > 0.1))
        {
            if (readyToPlayWalk)
            {
                // figure out what we're walking on
                RaycastHit rayQuery;
                Physics.Raycast(transform.position, Vector3.down, out rayQuery, playerHeight * 0.5f + 0.2f, whatIsGround);

                if(rayQuery.collider.tag == "Grass") // player is on grass
                {
                    playerAudioSource.PlayOneShot(walkingGrassSound, 1.0F); // walk on grass
                }
                else if (rayQuery.collider.tag == "Sand") // player is on sand
                {
                    playerAudioSource.PlayOneShot(walkingSandSound, 1.0F); // walk on sand
                }
                else
                {
                    playerAudioSource.PlayOneShot(walkingStoneSound, 1.0F); // default to stone
                }

                // need to delay sound affect
                readyToPlayWalk = false;
                Invoke(nameof(ResetWalkingSound), walkSoundCooldown); 
            }
            
        }
    }

    private void ResetWalkingSound()
    {
        readyToPlayWalk = true;
    }
}
