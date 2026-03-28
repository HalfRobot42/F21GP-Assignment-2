using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeDoor : MonoBehaviour
{
   //public GameObject errorUI;    // Drag "ErrorText" here
    public GameObject victoryUI;  // Drag "VictoryText" here

    public int nextSceneInt;
    public int currentSceneInt;

    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag("Player") && !levelFinished)
        {
            PlayerInventory inventory = other.GetComponentInParent<PlayerInventory>();

            if (inventory != null && inventory.hasSkull)
            {
                WinLevel();
            }
            else
            {
                // Show error message if they don't have the skull
                //if (errorUI != null) errorUI.SetActive(true);
            }
        }
        */

        if (other.CompareTag("Player"))
        {
            
            WinLevel();
            Invoke(nameof(nextScene), 1.0f); // reset game after 1 second
            //StartCoroutine(nextScene());
        }


    }

    /*
    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            // Hide the error message when they walk away from the door
            //if (errorUI != null) errorUI.SetActive(false);
        }
    }
    */

    void WinLevel()
    {
        
        // 1. Show Victory UI
        if (victoryUI != null) victoryUI.SetActive(true);

        // 2. Stop the Timer (If you have the Timer script on an object named "TimerManager")
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null) timer.enabled = false;

        //Debug.Log("Victory!");
    }

    
    void nextScene()
    {
        SceneManager.LoadScene(nextSceneInt, LoadSceneMode.Single); // go to next scene
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneInt));
        //SceneManager.UnloadSceneAsync(currentSceneInt);
    }

    //IEnumerator nextScene()
    //{
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadScene(sceneInt); // go to next scene
    //}

    /*
    public IEnumerator nextScene()
    {
        SceneManager.LoadScene(nextSceneInt, LoadSceneMode.Additive);
        var newScene = SceneManager.GetSceneByBuildIndex(nextSceneInt);
        while (!newScene.isLoaded)
        {
            yield return new WaitForSeconds(0.1f);
        }


        var currentScene = SceneManager.GetSceneByBuildIndex(currentSceneInt);
        if (currentScene.IsValid())
        {
            yield return SceneManager.UnloadSceneAsync(currentScene);
        }
        
        //SceneManager.LoadScene(nextSceneInt, LoadSceneMode.Additive);
        //var newScene = SceneManager.GetSceneByBuildIndex(nextSceneInt);
        //while (!newScene.isLoaded)
        //{
        //    yield return new WaitForSeconds(0.1f);
        //}
        
    }
    */
}
