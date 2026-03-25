using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    // define key pickup event here
    public static event Action OnPickup;
    public GameObject[] keySpawnPos;
    
    private int triggerEnterCount = 0;
    
    private void OnEnable()
    {
        TempleDoorOpen.OnDoorOpen += placeKey;
    }

    private void OnDisable()
    {
        TempleDoorOpen.OnDoorOpen += placeKey;
    }

    void Start()
    {
        if (keySpawnPos == null || keySpawnPos.Length == 0)
        {
            keySpawnPos = GameObject.FindGameObjectsWithTag("KeyRespawnPosition");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // use 
        if (other.CompareTag("Player") &&  triggerEnterCount == 0)
        {
            // trigger key pickup event
            OnPickup?.Invoke();
            //Debug.Log("Key is picked up");
            transform.parent.gameObject.SetActive(false); // disables GoldSkull (not destroyed)
            triggerEnterCount++;
            Debug.Log("Trigger has fired" + triggerEnterCount + " times");
        }
    }

    void placeKey()
    {
        // reactivate key
        transform.parent.gameObject.SetActive(true);
        // keySpawnPos is an array with one element where spawn position is
        transform.parent.position = keySpawnPos[0].transform.position;
    }
}
