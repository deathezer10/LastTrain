using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSlower : MonoBehaviour
{

    [SerializeField]
    private GameObject m_EscapeBlocker;

    TrainSpeedHandler m_SpeedHandler;
    StationMover m_StationMover;

    bool m_StartSlowing = false;
    bool m_GameEnded = false;

    private void Start()
    {
        m_SpeedHandler = FindObjectOfType<TrainSpeedHandler>();
        m_StationMover = FindObjectOfType<StationMover>();
        m_EscapeBlocker = GameObject.Find("EscapeDetector");
    }

    private void Update()
    {
        if (m_StartSlowing && !m_GameEnded && m_StationMover.currentSpeed <= 0)
        {
            m_GameEnded = true;
            m_EscapeBlocker.SetActive(false);

            AnnouncementManager.Instance.PlayAnnouncement2D("announcement_chime", AnnouncementManager.AnnounceType.Override, 0f);
            AnnouncementManager.Instance.StopAll();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Shell" && !m_StartSlowing)
        {
            m_SpeedHandler.enabled = false;
            m_StationMover.isMoving = false;
            m_StationMover.currentMaxSpeed = 10;
            m_StationMover.currentSpeed = 10;
            m_StationMover.m_StationAcceleration = 1.1f;
            m_StartSlowing = true;
        }
    }

}
