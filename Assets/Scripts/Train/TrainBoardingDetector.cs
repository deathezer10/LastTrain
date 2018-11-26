﻿using System.Collections;
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

    private ToggleTrainLights[] toggleTrainLights;

    void Start()
    {
        toggleTrainLights = FindObjectsOfType<ToggleTrainLights>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard += Time.deltaTime;

            // After player stands in the train for X seconds, close the doors and move the train
            if (m_HasMoved == false && m_PlayerTimeOnBoard >= m_PlatformMoveDelay)
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
                        foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
                        {
                            collider.enabled = true;
                            // FindObjectOfType<Bomb>().StartBomb();
                        }
                    });

                    m_TrainTimeHandler.StartTrainTime();
                    FindObjectOfType<TrainEscapeHandler>().TrainMoveStart();

                    AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
                    AnnouncementManager.Instance.PlayAnnouncement3D("thank_you", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
                    AnnouncementManager.Instance.PlayAnnouncement3D("bomb_intro", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 1f);
                });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard = 0;
        }
    }

}
