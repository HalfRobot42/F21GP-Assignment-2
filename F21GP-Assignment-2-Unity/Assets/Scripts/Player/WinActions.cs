using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinActions : MonoBehaviour
{

    private bool isPlayerInReach = false;
    private bool hasWon = false;

    public GameObject wonUI;
    public GameObject playerBody;
    public GameObject deathTeleport;
    public AudioClip wonSound;

    void Update()
    {
        if (isPlayerInReach && Input.GetKeyDown(KeyCode.E))
        {
            Win();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInReach = true;

            // Tell the UI Instance to show
            if (InteractionUI.Instance != null) InteractionUI.Instance.Show();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInReach = false;

            // Tell the UI Instance to hide
            if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();
        }
    }



    private void Win()
    {
        // move player to a remote location to avoid world noises
        playerBody.transform.position = deathTeleport.transform.position;

        if (!hasWon) // some behaviour we only want to perform once
        {
            // show won UI
            wonUI.SetActive(true);

            // play won sound
            AudioSource.PlayClipAtPoint(wonSound, playerBody.transform.position, 1.0F);

            hasWon = true;
        }
    }
}
