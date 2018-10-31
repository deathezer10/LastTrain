using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationDisplayUpdateSound : MonoBehaviour
{
    AudioPlayer updateSound;
    
    void Start()
    {
        updateSound = GetComponent<AudioPlayer>();
    }
    
    public void PlayUpdateSound()
    {
        if (!updateSound.IsPlaying())
        {
            updateSound.Play();
        }
    }
}
