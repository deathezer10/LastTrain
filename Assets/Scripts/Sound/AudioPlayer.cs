﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void Play()
    {
        InternalPlayClip();
    }

    public void Play(string clipAlias)
    {
        foreach (AudioPlayer player in transform.GetComponents<AudioPlayer>())
        {
            if (player.clipAlias == clipAlias)
            {
                player.InternalPlayClip();
                return;
            }
        }
    }

    AudioClip InternalPlayClip()
    {
        if (_clip == null) return null;

        m_Audiosource.pitch = randomizePitch ? UnityEngine.Random.Range(1.0f - pitchRandomRange, 1.0f + pitchRandomRange) : 1.0f;
        m_Audiosource.clip = clip;
        m_Audiosource.PlayDelayed(playDelay);

        return clip;
    }

    public bool IsPlaying() { return audioSource.isPlaying; }
}
