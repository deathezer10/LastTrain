﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(AudioSource))]
public class AnnouncementManager : SingletonMonoBehaviour<AnnouncementManager>
{
    #region Inspector Stuff
    [Serializable]
    private struct AnnouncementKeyPair
    {
        public string clipAlias;
        public AudioClip audioClip;
    }

    [SerializeField]
    AnnouncementKeyPair[] m_AnnouncementClips;

    Dictionary<string, AudioClip> m_AnnouncementClipsDictonary = new Dictionary<string, AudioClip>();
    #endregion

    private struct AnnouncementQueueInfo
    {
        public bool is3D;
        public AudioSource audioSource;
        public AudioClip audioClip;
        public float delayInSeconds;
        public string clipAlias;
    }

    Queue<AnnouncementQueueInfo> m_AnnouncementQueue = new Queue<AnnouncementQueueInfo>();

    AudioSource m_LocalAudioSource;
    AudioSource m_3DAudioSource;

    public enum AnnounceType
    {
        Queue,
        Override
    }

    private void Start()
    {
        // Add all elements into a dictionary for faster lookup
        foreach (var kp in m_AnnouncementClips)
        {
            m_AnnouncementClipsDictonary.Add(kp.clipAlias, kp.audioClip);
        }

        m_LocalAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayAnnouncement("cat", AnnounceType.Queue);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayAnnouncement("dog", AnnounceType.Override);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 pos = GameObject.FindWithTag("Player").transform.position;
            pos.z -= 5;
            PlayAnnouncementAt("duck", pos, AnnounceType.Override);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 pos = GameObject.FindWithTag("Player").transform.position;
            pos.z += 5;
            PlayAnnouncementAt("rabbit", pos, AnnounceType.Override);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 pos = GameObject.FindWithTag("Player").transform.position;
            PlayAnnouncementAt("cow", pos, AnnounceType.Queue);
        }
        */
    }
    
    /// <summary>
    /// Plays a 2D audio clip with the given alias
    /// </summary>
    public void PlayAnnouncement2D(string clipAlias, AnnounceType announceType, float delayInSeconds = 0)
    {
        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve an announcement's clipAlias");
            return;
        }

        switch (announceType)
        {
            case AnnounceType.Queue:
                m_AnnouncementQueue.Enqueue(new AnnouncementQueueInfo
                {
                    is3D = false,
                    audioSource = m_LocalAudioSource,
                    audioClip = clip,
                    clipAlias = clipAlias,
                    delayInSeconds = delayInSeconds
                });

                if (!m_LocalAudioSource.isPlaying && (m_3DAudioSource == null || m_3DAudioSource?.isPlaying == false))
                {
                    PlayNext();
                }
                break;

            case AnnounceType.Override:
                StopAll();
                m_LocalAudioSource.clip = clip;
                m_LocalAudioSource.PlayDelayed(delayInSeconds);
                StartCoroutine(PlayNextRoutine(m_LocalAudioSource.clip.length + delayInSeconds, false));
                break;

            default:
                Debug.LogError("Unhandled announcement type");
                break;
        }
    }

    /// <summary>
    /// Plays a 3D sound at the given position
    /// A GameObject is spawn at the given position and is removed once the audio finishes
    /// </summary>
    public void PlayAnnouncement3D(string clipAlias, Vector3 position, AnnounceType announceType, float delayInSeconds = 0)
    {
        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve an announcement's clipAlias");
            return;
        }

        GameObject obj = new GameObject("[3D_AnnouncementSound]");
        obj.transform.parent = transform;
        obj.transform.position = position;

        AudioSource spawnedAudioSource = obj.AddComponent<AudioSource>();
        spawnedAudioSource.clip = GetAudioClip(clipAlias);
        spawnedAudioSource.volume = 1;
        spawnedAudioSource.spatialBlend = 0.8f;
        spawnedAudioSource.minDistance = 1;
        spawnedAudioSource.maxDistance = 50;

        switch (announceType)
        {
            case AnnounceType.Queue:
                m_AnnouncementQueue.Enqueue(new AnnouncementQueueInfo
                {
                    is3D = true,
                    audioSource = spawnedAudioSource,
                    audioClip = clip,
                    clipAlias = clipAlias,
                    delayInSeconds = delayInSeconds
                });

                if (!m_LocalAudioSource.isPlaying && (m_3DAudioSource == null || m_3DAudioSource?.isPlaying == false))
                {
                    PlayNext();
                }
                break;

            case AnnounceType.Override:
                StopAll();
                m_3DAudioSource = spawnedAudioSource;
                m_3DAudioSource.PlayDelayed(delayInSeconds);
                StartCoroutine(PlayNextRoutine(m_3DAudioSource.clip.length + delayInSeconds, true));
                break;

            default:
                Debug.LogError("Unhandled announcement type");
                break;
        }
    }

    /// <summary>
    /// Stops the current announcement and plays the next one if any
    /// </summary>
    public void PlayNext()
    {
        if (m_AnnouncementQueue.Count == 0)
            return;
        else
        {
            m_LocalAudioSource.Stop();
            m_3DAudioSource?.Stop();

            var announceInfo = m_AnnouncementQueue.Dequeue();

            if (announceInfo.is3D)
                m_3DAudioSource = announceInfo.audioSource;

            AudioSource source = announceInfo.audioSource;
            source.clip = announceInfo.audioClip;
            source.PlayDelayed(announceInfo.delayInSeconds);
            StartCoroutine(PlayNextRoutine(source.clip.length + announceInfo.delayInSeconds, announceInfo.is3D));
        }
    }

    /// <summary>
    /// Erase all pending announcements in queue
    /// </summary>
    public void ClearQueue()
    {
        while (m_AnnouncementQueue.Count > 0)
        {
            var announceInfo = m_AnnouncementQueue.Dequeue();

            if (announceInfo.is3D)
                Destroy(announceInfo.audioSource.gameObject);
        }
    }

    public AudioClip GetAudioClip(string clipAlias)
    {
        AudioClip clip;
        m_AnnouncementClipsDictonary.TryGetValue(clipAlias, out clip);
        return clip;
    }

    /// <summary>
    /// Stops the current announcement and erase all pending announcements in queue
    /// </summary>
    public void StopAll()
    {
        m_LocalAudioSource.Stop();
        m_3DAudioSource?.Stop();
        ClearQueue();
        StopAllCoroutines();

        if (m_3DAudioSource != null)
        {
            Destroy(m_3DAudioSource.gameObject);
            m_3DAudioSource = null;
        }
    }

    private IEnumerator PlayNextRoutine(float delayInSeconds, bool is3D)
    {
        yield return new WaitForSeconds(delayInSeconds);

        if (is3D && m_3DAudioSource != null)
        {
            Destroy(m_3DAudioSource.gameObject);
            m_3DAudioSource = null;
        }

        PlayNext();
    }

}
