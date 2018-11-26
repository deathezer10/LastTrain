using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLeftAnnouncements : MonoBehaviour
{
    float timeBetweenEachStation, totalTrainTime;

    Transform playerTrans;

    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenEachStation = FindObjectOfType<TrainTimeHandler>().GetTimeBetween();

        totalTrainTime = timeBetweenEachStation * 5;

        StartCoroutine(TimeLeftRoutine());
    }

    private IEnumerator TimeLeftRoutine()
    {
        yield return new WaitForSeconds(totalTrainTime - 180f);

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("threeMinutes_remain", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 1f);
        yield return new WaitForSeconds(90f);

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("ninetySeconds_remain", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 1f);
        yield return new WaitForSeconds(60f);

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("thirtySeconds_remain", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 1f);
        yield return new WaitForSeconds(50f);

        AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("tenSeconds_remain", playerTrans.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 1f);
    }
}
