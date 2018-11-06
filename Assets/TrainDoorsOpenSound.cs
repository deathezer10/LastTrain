using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoorsOpenSound : MonoBehaviour {
    public List<AudioPlayer> audioPlayers;
    public bool bIsCabinPlaying { get; private set; } = false;
    public bool bIsDriverPlaying { get; private set; } = false;


    public float GetAudioLevel()
    {
       return audioPlayers[0].audioSource.volume;
    }

    public void SetAudioLevel(float val)
    {
        foreach(AudioPlayer audioplayer in audioPlayers)
        {
            audioplayer.audioSource.volume = val;
        }
    }



	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            audioPlayers.Add(transform.GetChild(i).GetComponent<AudioPlayer>());
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

   public void DriverDoorPlay()
    {
        bIsDriverPlaying = true;
        foreach (AudioPlayer audioplayer in audioPlayers)
        {
            audioplayer.audioSource.volume = (FindObjectOfType<StationMover>().currentSpeed / 10);
        }
        audioPlayers[0].Play();
    }

    public void CabinDoorsPlay()
    {
        bIsCabinPlaying = true;
        foreach (AudioPlayer audioplayer in audioPlayers)
        {
            audioplayer.audioSource.volume = (FindObjectOfType<StationMover>().currentSpeed / 10);
        }

        for (int i = 1; i < audioPlayers.Capacity; i++)
        {
            audioPlayers[i].Play();
        }
    }

    public void DriverDoorStopPlay()
    {
        bIsDriverPlaying = false;
        audioPlayers[0].Stop();
    }

    public void CabinDoorsStopPlay()
    {
        bIsCabinPlaying = false;
        for (int i = 1; i < audioPlayers.Capacity; i++)
        {
            audioPlayers[i].Stop();
        }
    }
}
