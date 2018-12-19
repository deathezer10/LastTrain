using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainBoardingDetector : MonoBehaviour
{

    public GameObject m_BombContainer;
    public TrainTimeHandler m_TrainTimeHandler;
    public TrainDoorHandler m_TrainDoorHandler;

    public StationMover m_StationMover;

    private const float m_PlatformMoveDelay = 3;

    private float m_PlayerTimeOnBoard;

    private bool m_HasMoved = false;
    private bool bDisabled = false;

    private ToggleTrainLights[] toggleTrainLights;

    void Start()
    {
        toggleTrainLights = FindObjectsOfType<ToggleTrainLights>();
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        m_PlayerTimeOnBoard += Time.deltaTime;
    //        transform.Find("LeaveStopper").gameObject.SetActive(true);
    //        // After player stands in the train for X seconds, close the doors and move the train
    //        if (m_HasMoved == false && m_PlayerTimeOnBoard >= m_PlatformMoveDelay)
    //        {

    //            for (int lightsFound = 0; lightsFound < toggleTrainLights.Length; lightsFound++)
    //            {
    //                toggleTrainLights[lightsFound].FlickerLights();
    //            }

    //            m_HasMoved = true;
    //            m_TrainDoorHandler.ToggleDoors(false, () =>
    //            {
    //                m_StationMover.ToggleMovement(true);

    //                m_BombContainer.SetActive(true);

    //                foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
    //                {
    //                    collider.enabled = false;
    //                }

    //                m_BombContainer.transform.DOLocalMoveY(0.5f, 1).OnComplete(() =>
    //                {
    //                    transform.Find("LeaveStopper").gameObject.SetActive(false);
    //                    Destroy(transform.Find("LeaveStopper")?.gameObject);
    //                    foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
    //                    {
    //                        collider.enabled = true;
    //                    }
    //                });

    //                m_TrainTimeHandler.StartTrainTime();
    //                FindObjectOfType<TimeLeftAnnouncements>().TrainMoveStart();
    //                FindObjectOfType<TrainEscapeHandler>().TrainMoveStart();

    //                AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
    //                AnnouncementManager.Instance.PlayAnnouncement3D("thank_you", AnnouncementManager.AnnounceType.Queue, 0f);
    //            });
    //        }
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (bDisabled) return;

        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard += Time.deltaTime;
        }


        if (m_HasMoved == false && m_PlayerTimeOnBoard >= m_PlatformMoveDelay)
        {
            nextStep();
            bDisabled = true;
        }



    }

    private void nextStep()
    {
        for (int lightsFound = 0; lightsFound < toggleTrainLights.Length; lightsFound++)
        {
            toggleTrainLights[lightsFound].FlickerLights();
        }

        m_HasMoved = true;
        m_TrainDoorHandler.ToggleDoors(false, () =>
        {
            m_StationMover.ToggleMovement(true);

            m_BombContainer.SetActive(true);

            foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            m_BombContainer.transform.DOLocalMoveY(0.5f, 1).OnComplete(() =>
            {
                transform.Find("LeaveStopper").gameObject.SetActive(false); 
                foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = true;
                }
                Destroy(transform.Find("LeaveStopper").gameObject);
                Destroy(this);
            });

            m_TrainTimeHandler.StartTrainTime();
            FindObjectOfType<TimeLeftAnnouncements>().TrainMoveStart();
            FindObjectOfType<TrainEscapeHandler>().TrainMoveStart();

            AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
            AnnouncementManager.Instance.PlayAnnouncement3D("thank_you", AnnouncementManager.AnnounceType.Queue, 0f);
           
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.Find("LeaveStopper").gameObject.SetActive(true);
        }

    }




    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard = 0;
            transform.Find("LeaveStopper").gameObject.SetActive(false);
        }
    }

}
