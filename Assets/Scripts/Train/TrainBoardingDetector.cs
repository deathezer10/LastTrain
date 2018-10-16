using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBoardingDetector : MonoBehaviour
{

    public TrainDoorHandler m_TrainDoorHandler;

    public PlatformTiler m_PlatformTiler;

    private const float m_PlatformMoveDelay = 5;

    private float m_PlayerTimeOnBoard;
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            m_PlayerTimeOnBoard += Time.deltaTime;

            // After player stands in the train for X seconds, close the doors and move the train
            if (m_PlayerTimeOnBoard >= m_PlatformMoveDelay)
            {
                m_TrainDoorHandler.ToggleDoors(false, () =>
                {
                    m_PlatformTiler.StartTiling();
                    Destroy(gameObject);
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
