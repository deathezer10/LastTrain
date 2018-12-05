using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorsManager : MonoBehaviour
{
    bool movingTriggered, stoppedTriggered;

    StationMover stationMover;

    private void Start()
    {
        stationMover = FindObjectOfType<StationMover>();
    }

    public void EmergencyDoorsTriggered()
    {
        if (!movingTriggered && stationMover.currentSpeed >= 1f)
        {
            movingTriggered = true;

            AnnouncementManager.Instance.PlayAnnouncement3D("emergencyDoors_moving", AnnouncementManager.AnnounceType.Queue, 0.5f);
        }
        else if (!stoppedTriggered && stationMover.currentSpeed <= 1f)
        {
            stoppedTriggered = true;

            AnnouncementManager.Instance.PlayAnnouncement3D("emergencyDoors_stopped", AnnouncementManager.AnnounceType.Queue, 0.5f);
        }
    }
}
