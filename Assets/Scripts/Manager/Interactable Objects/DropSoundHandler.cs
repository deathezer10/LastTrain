using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSoundHandler
{

    public enum DropSoundType
    {
        Generic,
        Metal,
        Wood,
        Plastic,
        Glass,
        None
    }

    public class ImpactNoiseData
    {
        public DropSoundType soundType = DropSoundType.Generic;

        // 0-1
        public float volume = 0.75f;

        // 0-1
        public float randomPitchRange = 0.2f;

        // 0-1
        public float spatialBlend = 1f;

        public float minDistance = 0.5f;

        public float maxDistance = 6f;
    }

    private ImpactNoiseData m_ImpactNoiseData = new ImpactNoiseData();

    private GameObject m_Source;

    private AudioSource m_AudioSource;

    private bool m_IsSetup = false;

    private bool m_IsEnabled = true;

    public DropSoundHandler(GameObject obj)
    {
        m_Source = obj;
    }

    public void SetImpactNoiseData(ImpactNoiseData data)
    {
        m_ImpactNoiseData = data;

        if (m_AudioSource != null)
            SetupAudioSource();
    }

    public void SetActive(bool active)
    {
        m_IsEnabled = active;
    }

    public void PlayDropSound(Vector3 relativeVelocity)
    {
        // Ignore empty sounds
        if (m_ImpactNoiseData.soundType == DropSoundType.None || m_IsEnabled == false)
            return;

        if (m_IsSetup == false)
        {
            GameObject obj = new GameObject("[DropSoundAudioSource]");
            obj.transform.parent = m_Source.transform;
            obj.transform.localPosition = Vector3.zero;
            m_AudioSource = obj.AddComponent<AudioSource>();
            SetupAudioSource();
            m_IsSetup = true;
        }

        const float minMagnitude = 1;
        const float maxMagnitude = 4;

        if (relativeVelocity.magnitude > minMagnitude)
        {
            m_AudioSource.pitch = 1 + Random.Range(-Mathf.Abs(m_ImpactNoiseData.randomPitchRange), Mathf.Abs(m_ImpactNoiseData.randomPitchRange));

            // Higher velocity = louder
            float volumeScale = (Mathf.Clamp(relativeVelocity.magnitude, minMagnitude, maxMagnitude) / maxMagnitude);

            // Uncomment to see which object is making noise
             Debug.LogFormat("Sound: {0}", m_Source.name);

            m_AudioSource.PlayOneShot(m_AudioSource.clip, volumeScale);
        }
    }

    private void SetupAudioSource()
    {
        m_AudioSource.clip = DropSoundClipManager.Instance.GetClip(m_ImpactNoiseData.soundType);
        m_AudioSource.volume = m_ImpactNoiseData.volume;
        m_AudioSource.spatialBlend = m_ImpactNoiseData.spatialBlend;
        m_AudioSource.minDistance = m_ImpactNoiseData.minDistance;
        m_AudioSource.maxDistance = m_ImpactNoiseData.maxDistance;
        m_AudioSource.rolloffMode = AudioRolloffMode.Linear;
    }
    
}
