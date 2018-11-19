using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoorsOpenSound : MonoBehaviour
{
    public List<AudioPlayer> audioPlayers;
    public bool bIsCabinPlaying { get; private set; } = false;
    public bool bIsDriverPlaying { get; private set; } = false;

    public AudioPlayer dingdongPlayer;
    public AudioPlayer trainEngine;
    

    public void SetAudioLevel(float val)
    {
        val = val / 10;
        foreach (AudioPlayer audioplayer in audioPlayers)
        {
            if (val > 0.8f) val = 0.8f;
            audioplayer.audioSource.volume = val;
        }
    }

    public void SetAudioLevelEngine(float val)
    {
        val = val / 10;
        trainEngine.audioSource.volume = val;
    }

    public void SetAudioLevelSpeed(float val)
    {
        if(val <= 10)
        trainEngine.audioSource.pitch = Mathf.Lerp(1, 1.8f, normalize01(val, 0, 10));

        else if(val > 10)
            trainEngine.audioSource.pitch = Mathf.Lerp(1.8f, 2.5f, normalize01(val, 10, 20));
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            audioPlayers.Add(transform.GetChild(i).GetComponent<AudioPlayer>());
        }
        trainEngine = transform.Find("TrainEngine").GetComponent<AudioPlayer>();
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
            audioplayer.audioSource.volume = (FindObjectOfType<StationMover>().currentSpeed / 10);
            if (audioplayer.audioSource.volume > 0.8f)
                audioplayer.audioSource.volume = 0.8f;
        }
        audioPlayers[0].Play();
    }

    public void CabinDoorsPlay()
    {
        bIsCabinPlaying = true;
        foreach (AudioPlayer audioplayer in audioPlayers)
        {
            audioplayer.audioSource.volume = (FindObjectOfType<StationMover>().currentSpeed / 10);
            if (audioplayer.audioSource.volume > 0.8f)
                audioplayer.audioSource.volume = 0.8f;
        }

        for (int i = 0; i < audioPlayers.Count; i++)
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

    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }
}
