using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PlayerInventory : MonoBehaviour
{
    public int keysCollected = 0;
    
    // 2. Add this variable to hold your UI Text
    public TMP_Text keyCounterText; 

    private void Start()
    {
        // Update the text at the very start so it shows 0/3
        UpdateKeyUI();
    }

    public void AddKey()
    {
        keysCollected++;
        // 3. Update the UI every time a key is added
        UpdateKeyUI();
         Debug.Log("Key collected! Total: " + keysCollected);

    }

    // A small function to handle the text update
    void UpdateKeyUI()
    {
        if (keyCounterText != null)
        {
            keyCounterText.text = "Keys: " + keysCollected + " / 3";
        }
    }
}
