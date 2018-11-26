using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorsManager : MonoBehaviour
{
    Transform playerTrans;
    bool movingTriggered, stoppedTriggered;

    StationMover stationMover;

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        stationMover = FindObjectOfType<StationMover>();
    }

    public void EmergencyDoorsTriggered()
    {
        if (!movingTriggered && stationMover.currentSpeed >= 1f)
        {
            movingTriggered = true;

            AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
            AnnouncementManager.Instance.PlayAnnouncement3D("emergencyDoors_moving", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0.5f);
        }
        else if (!stoppedTriggered && stationMover.currentSpeed <= 1f)
        {
            stoppedTriggered = true;

            AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
            AnnouncementManager.Instance.PlayAnnouncement3D("emergencyDoors_stopped", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0.5f);
        }
    }
}
