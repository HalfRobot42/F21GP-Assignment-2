using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerInventory : MonoBehaviour
{
    public int keysCollected = 0;
    public bool hasSkull = false;
    
    // Added variable to hold the UI Text
    public TMP_Text keyCounterText; 

    private void Start()
    {
        // Updates the text at the very start so it shows present number of keys (0/3)
        UpdateKeyUI();
    }

    public void AddKey()
    {
        keysCollected++;
        //Updates the UI every time a key is added
        UpdateKeyUI();
        Debug.Log("Key collected! Total: " + keysCollected);

    }

    // Handles the text update
    void UpdateKeyUI()
    {
        if (keyCounterText != null)
        {
            keyCounterText.text = "Keys: " + keysCollected + " / 3";
        }
    }

    // Function to collect the Egg
    public void CollectSkull()
    {
        hasSkull = true;
        Debug.Log("Skull Collected!");
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification("You found the Skull!");
    }
}
