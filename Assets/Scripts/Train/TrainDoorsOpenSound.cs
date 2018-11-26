using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoorsOpenSound : MonoBehaviour
{
    public List<AudioPlayer> audioPlayers;
    public bool bIsCabinPlaying { get; private set; } = false;
    public bool bIsDriverPlaying { get; private set; } = false;
    private StationMover stationMover;
    public AudioPlayer dingdongPlayer;
    public AudioPlayer trainEngine;
    private float windMaxVolume = 0.8f;
    

    public void SetWindAudioLevel(float val) //wind volume
    {
       val = Mathf.Clamp(val, 0, 10);

        if (val != 0)
            val = val / 10;
        else
            val = 0;

       foreach(AudioPlayer audioplayer in audioPlayers )
        {
            audioplayer.audioSource.volume = val;
        }
       
    }

    public void SetAudioLevelPitch(float val) //pitch of engine sound
    {
        if(val <= 10)
        trainEngine.audioSource.pitch = Mathf.Lerp(1, 1.8f, normalize01(val, 0, 10));

        else if(val > 10)
            trainEngine.audioSource.pitch = Mathf.Lerp(1.8f, 2.5f, normalize01(val, 10, 20));
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag == "Cabin1" || transform.GetChild(i).tag == "Cabin2" || transform.GetChild(i).tag == "drivercabin")
            audioPlayers.Add(transform.GetChild(i).GetComponent<AudioPlayer>());
        }
        trainEngine = transform.Find("TrainEngine").GetComponent<AudioPlayer>();
        stationMover = FindObjectOfType<StationMover>();
        trainEngine.Play();
    }

    public void PlayDingDong()
    {
        dingdongPlayer.Play("dingdong");
    }

    public void DriverDoorPlay()
    {
        bIsDriverPlaying = true;
        foreach (AudioPlayer audioplayer in audioPlayers)
        {

            if (audioplayer.tag == "drivercabin")
            {
                audioplayer.Play();
                if (stationMover.currentSpeed != 0)
                    audioplayer.audioSource.volume = stationMover.currentSpeed / 10;
                else
                    audioplayer.audioSource.volume = 0;

                if (audioplayer.audioSource.volume > 0.8f)
                    audioplayer.audioSource.volume = 0.8f;

                break;
            }
        } 
    }

    public void CabinDoorsPlay(string _tag)
    {
        foreach (AudioPlayer audioplayer in audioPlayers)
        {
            if(audioplayer.tag == _tag)
            {
                audioplayer.audioSource.volume = stationMover.currentSpeed / 10;
                if (audioplayer.audioSource.volume > 0.8f)
                    audioplayer.audioSource.volume = 0.8f;

                audioplayer.Play();
            }           
        }

    }

    public void DriverDoorStopPlay()
    {
        for(int i = 1; i < audioPlayers.Count; i++)
        {
            if (audioPlayers[i].tag == "drivercabin")
                audioPlayers[i].Stop();
        }
    }

    public void CabinDoorsStopPlay(string _tag)
    {
        for (int i = 1; i < audioPlayers.Count; i++)
        {
            if(audioPlayers[i].tag == _tag)
            audioPlayers[i].Stop();
        }
    }

    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }
}
