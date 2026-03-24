using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
   public GameObject errorUI;    // Drag "ErrorText" here
    public GameObject victoryUI;  // Drag "VictoryText" here
    
    private bool levelFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !levelFinished)
        {
            PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();

            if (inventory != null && inventory.hasEgg)
            {
                WinLevel();
            }
            else
            {
                // Show error message if they don't have the egg
                if (errorUI != null) errorUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the error message when they walk away from the door
            if (errorUI != null) errorUI.SetActive(false);
        }
    }

    void WinLevel()
    {
        levelFinished = true;
        
        // 1. Show Victory UI
        if (victoryUI != null) victoryUI.SetActive(true);
        if (errorUI != null) errorUI.SetActive(false);

        // 2. Stop the Timer (If you have the Timer script on an object named "TimerManager")
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null) timer.enabled = false;

        // 3. Disable Player Movement so they can't keep walking
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = false;

        Debug.Log("Victory!");
    }
}
