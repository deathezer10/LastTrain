using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMover : MonoBehaviour
{

    public TrainDoorHandler m_TrainDoorHandler;

    private const float m_MaxTrainMoveSpeed = 10;

    private const float m_TrainMoveDelay = 5;

    private float m_PlayerTimeOnBoard;

    private float m_CurrentTrainSpeed = 0;

    enum TrainState
    {
        Stationary,
        Moving
    }

    private TrainState m_CurrentTrainState = TrainState.Stationary;

    private void Update()
    {
        if (m_CurrentTrainState == TrainState.Moving)
        {
            m_CurrentTrainSpeed = Mathf.Clamp(m_CurrentTrainSpeed + Time.deltaTime, 0, m_MaxTrainMoveSpeed);
            transform.parent.Translate(Vector3.forward * m_CurrentTrainSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard += Time.deltaTime;

            // After player stands in the train for X seconds, close the doors and move the train
            if (m_CurrentTrainState == TrainState.Stationary && m_PlayerTimeOnBoard >= m_TrainMoveDelay)
            {
                m_TrainDoorHandler.ToggleDoors(false, () =>
                {
                    m_CurrentTrainState = TrainState.Moving;
                    GameObject.FindWithTag("Player").transform.SetParent(transform.parent);
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
