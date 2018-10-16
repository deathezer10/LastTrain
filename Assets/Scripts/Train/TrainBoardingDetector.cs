using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBoardingDetector : MonoBehaviour
{

    public TrainDoorHandler m_TrainDoorHandler;

    public StationMover m_StationMover;

    private const float m_PlatformMoveDelay = 5;

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
