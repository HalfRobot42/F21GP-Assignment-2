using UnityEngine;
using UnityEngine.AI;

public class SlidingDoor : MonoBehaviour
{
    public Transform doorTransform; 
    public GameObject lockedPrompt; // Drag your "LockedDoorPrompt" UI here
    
    [Header("Movement Settings")]
    public Vector3 openOffset = new Vector3(5f, 0f, 0f);
    public float speed = 3f;
    public int keysRequired = 3;

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private NavMeshObstacle navObstacle;

    void Start()
    {
        if (doorTransform == null) doorTransform = transform.parent;
        closedPosition = doorTransform.position;
        targetPosition = closedPosition;
        navObstacle = doorTransform.GetComponent<NavMeshObstacle>();
        
        // Ensure the prompt is hidden at the start
        if (lockedPrompt != null) lockedPrompt.SetActive(false);
    }

    void Update()
    {
        if (doorTransform == null) return;
        doorTransform.position = Vector3.MoveTowards(doorTransform.position, targetPosition, speed * Time.deltaTime);
        
        if (navObstacle != null)
            navObstacle.carving = (Vector3.Distance(doorTransform.position, targetPosition) < 0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();
            
            // If player has enough keys, open the door
            if (inventory != null && inventory.keysCollected >= keysRequired)
            {
                targetPosition = closedPosition + openOffset;
                if (lockedPrompt != null) lockedPrompt.SetActive(false);
                Debug.Log("Door Unlocked!");
            }
            // If NOT enough keys, show the "Locked" message
            else
            {
                if (lockedPrompt != null) lockedPrompt.SetActive(true);
                Debug.Log("Door is Locked! Needs " + keysRequired + " keys.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Always hide the message when the player walks away
            if (lockedPrompt != null) lockedPrompt.SetActive(false);

            // Close the door (Only if it's currently open)
            targetPosition = closedPosition;
        }
    }
}