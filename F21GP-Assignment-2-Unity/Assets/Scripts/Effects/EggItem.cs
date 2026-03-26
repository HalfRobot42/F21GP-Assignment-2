using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggItem : MonoBehaviour
{
   
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory inv = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerInventory>();
            inv.CollectSkull();
            
            if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (InteractionUI.Instance != null) InteractionUI.Instance.Show();
        }
    }
}
