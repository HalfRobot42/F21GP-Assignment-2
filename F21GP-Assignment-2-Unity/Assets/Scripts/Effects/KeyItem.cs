using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private bool isPlayerInReach = false;
    private PlayerInventory playerInv;
    public AudioClip pickupSound;

    void Update()
    {
        if (isPlayerInReach && Input.GetKeyDown(KeyCode.E))
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
            playerInv.AddKey();

            // pickup sound
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, 1.0F);

            // Tell the UI Instance to hide
            if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();

            Debug.Log("Key Picked Up!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
}