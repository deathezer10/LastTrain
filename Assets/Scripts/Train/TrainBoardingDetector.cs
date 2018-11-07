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
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard += Time.deltaTime;

            // After player stands in the train for X seconds, close the doors and move the train
            if (m_HasMoved == false && m_PlayerTimeOnBoard >= m_PlatformMoveDelay)
            {
                m_HasMoved = true;
                m_TrainDoorHandler.ToggleDoors(false, () =>
                {
                    m_StationMover.ToggleMovement(true);

                    m_BombContainer.SetActive(true);

                    foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = false;
                    }

                    m_BombContainer.transform.DOLocalMoveY(0.5f, 2).OnComplete(() =>
                    {
                        foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
                        {
                            collider.enabled = true;
                        }
                    });

                    m_TrainTimeHandler.StartTrainTime();
                    FindObjectOfType<TrainEscapeHandler>().TrainMoveStart();
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
