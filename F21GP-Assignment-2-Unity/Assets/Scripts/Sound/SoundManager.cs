using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource musicSource;

    public AudioSource soundEffectSource;

    public AudioClip levelMusic;

    public AudioClip pickupSound;

    public AudioClip enemyDeathSound;

    public AudioClip buttonClickSound;
    public AudioClip gameOverSound;

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

    /*
    This method plays the music in the game.
    */
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    /*
    This method plays a sound effect in the game.
    */
    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }
    
}
