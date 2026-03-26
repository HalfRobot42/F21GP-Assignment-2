using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    // define key pickup event here
    public static event Action OnPickup;
    public GameObject[] keySpawnPos;

    private bool isPlayerInReach = false;
    private PlayerInventory playerInv;

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

    /*
    void OnTriggerEnter(Collider other)
    {
        // use 
        if (other.CompareTag("Player") &&  triggerEnterCount == 0)
        {
            // trigger key pickup event
            OnPickup?.Invoke();
            Debug.Log("Key is picked up");
            transform.parent.gameObject.SetActive(false); // disables GoldSkull (not destroyed)
            triggerEnterCount++;
            Debug.Log("Trigger has fired" + triggerEnterCount + " times");
        }
    }
    */

    void Update()
    {
        if (isPlayerInReach && Input.GetKeyDown(KeyCode.E) && triggerEnterCount == 0)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        // Look for the inventory on the Player (the parent of the capsule)
        if (playerInv == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerInv = p.GetComponentInParent<PlayerInventory>();
        }

        if (playerInv != null)
        {
            playerInv.CollectSkull();

            // Tell the UI Instance to hide
            if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();

            Debug.Log("Skull Picked Up!");

            // trigger key pickup event
            OnPickup?.Invoke();
            transform.parent.gameObject.SetActive(false); // disables GoldSkull (not destroyed)
            triggerEnterCount++;
            Debug.Log("Trigger has fired" + triggerEnterCount + " times");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && triggerEnterCount == 0)
        {
            isPlayerInReach = true;

            // Tell the UI Instance to show
            if (InteractionUI.Instance != null) InteractionUI.Instance.Show();

            Debug.Log("Player is near the key. UI should be visible.");
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

    void placeKey()
    {
        // reactivate key
        transform.parent.gameObject.SetActive(true);
        // keySpawnPos is an array with one element where spawn position is
        transform.parent.position = keySpawnPos[0].transform.position;
    }
}
