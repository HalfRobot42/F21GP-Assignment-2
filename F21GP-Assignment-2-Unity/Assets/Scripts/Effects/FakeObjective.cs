using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeObjective : MonoBehaviour
{
    public GameObject fakeMessageUI; 
    public float displayTime = 3f;   

    private bool isPlayerInRange = false;
    private bool messageShowing = false;

    // Start is called before the first frame update
    void Start()
    {
        if (fakeMessageUI != null) fakeMessageUI.SetActive(false);
    
    }

    // Update is called once per frame
    void Update()
    {
         // If player is near and presses E
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !messageShowing)
        {
            ShowFakeMessage();
        }
    }

    void ShowFakeMessage()
    {
        messageShowing = true;
        
        // Show the warning text
        if (fakeMessageUI != null) fakeMessageUI.SetActive(true);

        // Optional: Hide the "[E] Pick Up" prompt from the InteractionUI system
        if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();

        // Optional: Trigger your NotificationManager if you have one
        if (NotificationManager.Instance != null)
            NotificationManager.Instance.ShowNotification("It's a trap!");

        // Hide the message automatically after a few seconds
        Invoke("HideMessage", displayTime);
    }

    void HideMessage()
    {
        if (fakeMessageUI != null) fakeMessageUI.SetActive(false);
        messageShowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show the standard "[E] Pick Up" prompt
            if (InteractionUI.Instance != null) InteractionUI.Instance.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (InteractionUI.Instance != null) InteractionUI.Instance.Hide();
            HideMessage(); // Hide the warning if they walk away
        }
    }
}
