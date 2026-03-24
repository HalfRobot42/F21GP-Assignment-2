using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    // define key pickup event here
    public static event Action OnPickup;
    void OnTriggerEnter(Collider other)
    {
        // use 
        if (other.CompareTag("Player"))
        {
            // trigger key pickup event
            OnPickup?.Invoke();
            //Debug.Log("Key is picked up");
            transform.parent.gameObject.SetActive(false); // disables GoldSkull (not destroyed)
        }
    }
    
}
