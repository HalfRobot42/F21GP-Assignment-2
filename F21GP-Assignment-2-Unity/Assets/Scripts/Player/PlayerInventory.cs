using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerInventory : MonoBehaviour
{
    public int keysCollected = 0;
    public bool hasSkull = false;
    public bool needsKeys = true; // does this level have keys?
    
    // Added variable to hold the UI Text
    public TMP_Text keyCounterText;
    public TMP_Text skullCounterText;

    private void Start()
    {
        // Updates the text at the very start so it shows present number of keys (0/3)
        UpdateKeyUI();

        if (!needsKeys) // if key collection is disabled, hide key ui
        {
            keyCounterText.text = "";
        }
    }

    public void AddKey()
    {
        keysCollected++;
        //Updates the UI every time a key is added
        UpdateKeyUI();
        //Debug.Log("Key collected! Total: " + keysCollected);

    }

    // Handles the text update
    void UpdateKeyUI()
    {
        if (needsKeys)
        {
            keyCounterText.text = "Keys: " + keysCollected + " / 3";
        }
    }

    // Function to collect the Egg
    public void CollectSkull()
    {
        hasSkull = true;
        skullCounterText.text = "Skull: 1 / 1";
        //Debug.Log("Skull Collected!");
        //if (NotificationManager.Instance != null)
        //    NotificationManager.Instance.ShowNotification("You found the Skull!");
    }
}
