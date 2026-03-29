using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
This script is for the timer that will be used in the game. It will be used
to keep track of the time the player has to escape a temple.

*/
public class Timer : MonoBehaviour
{
    public float timeLimit = 60f;

    public TextMeshProUGUI timeDisplay;
    private bool timerRunning = true;
  

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if (timeLimit > 0)
            {
                timeLimit -= Time.deltaTime;
                timeLimit = Mathf.Max(timeLimit, 0); // ensure time ends at 0:00
                UpdateTimerDisplay(timeLimit);    
            }

            else
            {
                timeLimit = 0;
                timerRunning = false;
                NotificationManager.Instance.ShowNotification("Game Over! You failed to escape the temple in time.");
            }
            

            
        }
    }

    void UpdateTimerDisplay(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timeDisplay.text = string.Format("{0:00}: {1:00}", minutes, seconds);
    }
}
