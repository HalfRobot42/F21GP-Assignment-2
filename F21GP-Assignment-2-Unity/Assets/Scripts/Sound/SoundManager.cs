using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // This "Static Instance" allows any script to find this UI instantly
    //public static SoundManager Instance;

    public AudioSource levelAmbianceSource;

    public AudioClip levelAmbiance;

    private void Start()
    {
        //PlayAmbiance();
    }

    /*
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
    */

    /*
    This method plays the music in the game.
    */
    public void PlayAmbiance()
    {
        levelAmbianceSource.clip = levelAmbiance;
        levelAmbianceSource.loop = true;
        levelAmbianceSource.Play();
    }

    /*
    This method plays a sound effect in the game.
    */
    public void PlaySoundEffect(AudioClip clip, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos);
    }
    
}
