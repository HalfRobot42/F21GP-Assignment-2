using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
This script is for the notifications that will be used in the game.
*/
public class NotificationManager : MonoBehaviour
{

    public static NotificationManager Instance; // singleton instance of the notification manager

    public TextMeshProUGUI notificationText;
    [SerializeField] public float displayTime = 5f; // display time for the notification

    private Coroutine currentNotificationDisplayed;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);       
        }
        else
        {
            Destroy(gameObject);
        }
    }


    IEnumerator DisplayNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayTime);
        notificationText.gameObject.SetActive(false);
    }

    /*
    This methods shows a notification on the screen for a period of time.
    If a notification already exists, it will be replaced by the new notification.
    */
    public void ShowNotification(string message)
    {
        if (currentNotificationDisplayed != null)
        {
            StopCoroutine(currentNotificationDisplayed);
        }
        
        currentNotificationDisplayed = StartCoroutine(DisplayNotification(message));
    }
}
