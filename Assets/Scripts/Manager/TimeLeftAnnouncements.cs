using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLeftAnnouncements : MonoBehaviour
{
    float timeBetweenEachStation, totalTrainTime;

    void Start()
    {
        timeBetweenEachStation = FindObjectOfType<TrainTimeHandler>().GetTimeBetween();

        totalTrainTime = timeBetweenEachStation * 5;
    }

    public void TrainMoveStart()
    {
        StartCoroutine(TimeLeftRoutine());
    }

    [SerializeField]
    private StationMover _mover;

    private IEnumerator TimeLeftRoutine()
    {
        yield return new WaitForSeconds(totalTrainTime - 180f);

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("threeMinutes_remain", AnnouncementManager.AnnounceType.Queue, 0f);
        yield return new WaitForSeconds(90f);

        if (!isAnnouncement()) yield break;

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("ninetySeconds_remain", AnnouncementManager.AnnounceType.Queue, 0f);
        yield return new WaitForSeconds(60f);

        if (!isAnnouncement()) yield break;

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("thirtySeconds_remain", AnnouncementManager.AnnounceType.Queue, 0f);
        yield return new WaitForSeconds(20f);

        if (!isAnnouncement()) yield break;

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("tenSeconds_remain", AnnouncementManager.AnnounceType.Queue, 0f);
    }

    private bool isAnnouncement()
    {
        if (_mover.currentSpeed == 0)
        {
            return false;
        }
        return true;
    }
}
