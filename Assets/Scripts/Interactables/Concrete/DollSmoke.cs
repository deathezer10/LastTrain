using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollSmoke : MonoBehaviour
{
    AudioPlayer m_BurnAudio;

    SmokeDetector m_CurrentSmokeDetector;


    private void OnEnable()
    {
        m_BurnAudio = GetComponent<AudioPlayer>();
        m_BurnAudio.Play();
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        if (m_CurrentSmokeDetector != null)
        {
            m_CurrentSmokeDetector.DetectingSmoke(false);
            m_CurrentSmokeDetector = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SmokeDetector")
        {
            m_CurrentSmokeDetector = other.GetComponent<SmokeDetector>();
            m_CurrentSmokeDetector.DetectingSmoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SmokeDetector")
        {
            if (m_CurrentSmokeDetector != null)
            {
                m_CurrentSmokeDetector.DetectingSmoke(false);
                m_CurrentSmokeDetector = null;
            }
        }
    }

}
