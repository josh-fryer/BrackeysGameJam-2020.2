using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] float fadingTime = 0.3f;
    private float startVolume;
    private float volume1;
    
    public bool fadeOut = false;
    public bool fadeIn = false;

    private AudioSource audioSource;
    
    void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
        startVolume = audioSource.volume;
        volume1 = startVolume;
        audioSource.volume = 0f;
        fadeIn = true;  
    }

    void Start()
    {
        //audioSource.Play();
              
 
    }

    void Update() 
    {
        if (fadeOut)
        {
            FadeOutMusic();
        }

        if (fadeIn)
        {
            FadeInMusic();
        }
    }

    public void FadeOutMusic()
    {
        Debug.Log("Fade out music");
        volume1 -= fadingTime * Time.deltaTime;
        audioSource.volume = volume1;
        if (audioSource.volume <= 0)
        {
            Debug.Log("Fade out is false");
            fadeOut = false;
            volume1 = startVolume;
        }
    }

    public void FadeInMusic()
    {
        Debug.Log("Fade in music");
       audioSource.volume += fadingTime * Time.deltaTime;
        
        if (audioSource.volume >= startVolume)
        {
            Debug.Log("Finished fade in");
            fadeIn = false;
        }
    }
}
