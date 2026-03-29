using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMonitor : MonoBehaviour
{

    public GameObject playerBody;
    public GameObject deathUI;
    public GameObject timer;
    public GameObject deathTeleport;
    public AudioClip deathSound;
    public int currentScene;

    private bool diedOnce = false;

    // Update is called once per frame
    void Update()
    {
        if (playerBody.GetComponent<PlayerControl>().health <= 0 || timer.GetComponent<Timer>().timeLimit <= 0)
        {
            // player has died

            // move player to a remote location to avoid world noises
            playerBody.transform.position = deathTeleport.transform.position;

            if (!diedOnce) // some behaviour we only want to perform once
            {
                // show death UI
                deathUI.SetActive(true);

                // play death sound
                AudioSource.PlayClipAtPoint(deathSound, playerBody.transform.position, 1.0F);

                // reload scene
                Invoke(nameof(respawn), 5.0f); // reset game delay

                diedOnce = true;
            }
        }
    }

    void respawn()
    {
        SceneManager.LoadScene(currentScene, LoadSceneMode.Single); // go to next scene
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextSceneInt));
        //SceneManager.UnloadSceneAsync(currentSceneInt);
    }
}
