using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleDoorOpen : MonoBehaviour
{
    private bool isOpen = false;

    public static event Action OnDoorOpen;
    
    private int triggerEnterCount = 0;
    private void OnEnable()
    {
        KeyPickup.OnPickup += setUnlockBool;
    }

    private void OnDisable()
    {
        KeyPickup.OnPickup -= setUnlockBool;
    }

    void setUnlockBool()
    {
        if (!isOpen)
        {
            isOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isOpen == true && triggerEnterCount == 0)
        {
            OnDoorOpen?.Invoke();
            Debug.Log("Key is picked up and door can be opened.");
            // ensures door only opens once
            triggerEnterCount++;
        }
    }
    
    
}
