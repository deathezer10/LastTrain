using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    private bool randomizePitch = true;

    [SerializeField]
    private float pitchRandomRange = 0.2f;

    [SerializeField]
    private float playDelay = 0;

    [SerializeField]
    private AudioClip _clip;

    public string clipAlias = "";

    protected AudioSource m_Audiosource;

    public AudioSource audioSource { get { return m_Audiosource; } }

    public AudioClip clip {
        get { return _clip; }
        private set { _clip = value; }
    }

    void Awake()
    {
        m_Audiosource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _clip.LoadAudioData();
    }

    private void OnDisable()
    {
        _clip.UnloadAudioData();
    }

    public void Stop()
    {
        m_Audiosource.Stop();
    }

    public void Play(Action onPlayFinished = null)
    {
        InternalPlayClip(onPlayFinished);
    }

    public AudioPlayer Play(string clipAlias, Action onPlayFinished = null, float delaySeconds = 0)
    {
        var audioPlayer = GetLocalAudioPlayer(clipAlias);

        if (audioPlayer != null)
        {
            audioPlayer.InternalPlayClip(onPlayFinished, delaySeconds);
            return audioPlayer;
        }
        else
        {
            return null;
        }

    }

    public AudioPlayer GetLocalAudioPlayer(string clipAlias)
    {
        foreach (AudioPlayer player in transform.GetComponents<AudioPlayer>())
        {
            if (player.clipAlias == clipAlias)
                return player;
        }
        return null;
    }

    AudioClip InternalPlayClip(Action onPlayFinished = null, float delaySeconds = 0)
    {
        if (_clip == null) return null;

        m_Audiosource.pitch = randomizePitch ? UnityEngine.Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : m_Audiosource.pitch;
        m_Audiosource.clip = clip;
        m_Audiosource.PlayDelayed(playDelay);

        if (onPlayFinished != null)
        {
            StartCoroutine(WaitUntilAudioFinishes(onPlayFinished, delaySeconds));
        }

        return clip;
    }

    IEnumerator WaitUntilAudioFinishes(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(clip.length + playDelay + delaySeconds);
        action?.Invoke();
    }

    public bool IsPlaying() { return audioSource.isPlaying; }
}
