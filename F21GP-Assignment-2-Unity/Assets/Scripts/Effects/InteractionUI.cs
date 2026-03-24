using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    // This "Static Instance" allows any script to find this UI instantly
    public static InteractionUI Instance;

    void Awake()
    {
        // Register this specific object as the "Instance"
        Instance = this;
        
        // Hide the UI at the very start
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}