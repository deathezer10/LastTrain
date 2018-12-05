using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawn Gameobjects for playing "train announcement" Sound Sources,
/// Reference for each announcement sound file set in Inspector with a string alias
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AnnouncementManager : SingletonMonoBehaviour<AnnouncementManager>
{
    #region References to be set in Inspector

    // Pairing audio assets to strings for clearer references
    [Serializable]
    private struct AnnouncementKeyPair
    {
        public string clipAlias;
        public AudioClip audioClip;
    }

    [SerializeField]
    AnnouncementKeyPair[] m_AnnouncementClips;

    // References to plushie dolls in the scene for triggering their visual effects when an announcement plays
    [SerializeField]
    Doll[] m_Dolls;

    [SerializeField]
    Transform m_PlayerTrans;

    Dictionary<string, AudioClip> m_AnnouncementClipsDictonary = new Dictionary<string, AudioClip>();

    #endregion

    private struct AnnouncementQueueInfo
    {
        public bool bIs3D;
        public AudioSource audioSource;
        public AudioClip audioClip;
        public float delayInSeconds;
        public string clipAlias;
    }

    Queue<AnnouncementQueueInfo> m_AnnouncementQueue = new Queue<AnnouncementQueueInfo>();

    AudioSource m_LocalAudioSource;
    AudioSource m_3DAudioSource;

    GameObject m_3DGameObject;

    Vector3 m_3DClipPosOffset = new Vector3(0, 15f, 0);

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

    /// <summary>
    /// Plays a 2D audio clip with the given alias
    /// </summary>
    public void PlayAnnouncement2D(string clipAlias, AnnounceType announceType, float delayInSeconds = 0)
    {
        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve announcement with alias: " + clipAlias);
            return;
        }

        ActivatePlushieEyes(clip.length);

        switch (announceType)
        {
            case AnnounceType.Queue:
                m_AnnouncementQueue.Enqueue(new AnnouncementQueueInfo
                {
                    bIs3D = false,
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
    /// Plays a 3D sound above the players position with the given alias
    /// </summary>
    /// <returns>Reference to the AudioSource that was spawned</returns>
    public GameObject PlayAnnouncement3D(string clipAlias, AnnounceType announceType, float delayInSeconds = 0)
    {
        // Only add the new announcement chime to queue if the queue is empty
        if (m_AnnouncementQueue.Count != 0 && clipAlias == "announcement_chime")
        {
            return null;
        }

        AudioClip clip = GetAudioClip(clipAlias);

        if (clip == null)
        {
            Debug.LogError("Error trying to retrieve announcement with alias: " + clipAlias);
            return null;
        }

        m_3DGameObject = new GameObject("[3D_AnnouncementSound]");
        m_3DGameObject.transform.parent = transform;

        AudioSource spawnedAudioSource = m_3DGameObject.AddComponent<AudioSource>();
        spawnedAudioSource.clip = GetAudioClip(clipAlias);
        spawnedAudioSource.volume = 1f;
        spawnedAudioSource.spatialBlend = 1f;
        spawnedAudioSource.minDistance = 1f;
        spawnedAudioSource.maxDistance = 70f;
        spawnedAudioSource.rolloffMode = AudioRolloffMode.Linear;

        // Check if the announcement should skip the queue or be added to it
        switch (announceType)
        {
            case AnnounceType.Queue:
                m_AnnouncementQueue.Enqueue(new AnnouncementQueueInfo
                {
                    bIs3D = true,
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

        return m_3DGameObject;
    }

    /// <summary>
    /// Activates the Eye Flash effect for each cat plushie still alive
    /// </summary>
    private void ActivatePlushieEyes(float clipLength)
    {
        foreach (Doll doll in m_Dolls)
        {
            if (doll != null)
            {
                doll.StartEyeFlash(clipLength);
            }
        }
    }

    /// <summary>
    /// Stops the current announcement and plays the next one if any
    /// </summary>
    public void PlayNext()
    {
        if (m_AnnouncementQueue.Count == 0)
        {
            return;
        }
        else
        {
            m_LocalAudioSource.Stop();
            m_3DAudioSource?.Stop();

            var announceInfo = m_AnnouncementQueue.Dequeue();

            if (announceInfo.bIs3D)
            {
                m_3DAudioSource = announceInfo.audioSource;
            }

            m_3DGameObject.transform.position = m_PlayerTrans.position + m_3DClipPosOffset;

            AudioSource source = announceInfo.audioSource;
            source.clip = announceInfo.audioClip;

            ActivatePlushieEyes(source.clip.length);

            source.PlayDelayed(announceInfo.delayInSeconds);
            StartCoroutine(PlayNextRoutine(source.clip.length + announceInfo.delayInSeconds, announceInfo.bIs3D));
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

            if (announceInfo.bIs3D)
            {
                Destroy(announceInfo.audioSource.gameObject);
            }
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

    /// <summary>
    /// Delay routine for the next clip if there is a queue
    /// </summary>
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