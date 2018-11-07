using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEscapeHandler : MonoBehaviour
{
    BoxCollider[] colliders;
    StationMover stationMover;

    private void Start()
    {
        stationMover = FindObjectOfType<StationMover>();

        colliders = GetComponents<BoxCollider>();

        foreach (BoxCollider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exited the train, train was moving at a speed of " + stationMover.currentSpeed);

            if (stationMover.currentSpeed <= 1f)
            {
                Debug.Log("Player victory.");
                //FindObjectOfType<PlayerVictoryHandler>().PlayerVictory();
            }
            else
            {
                Debug.Log("Player death.");
                //FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainjump");
            }
        }
    }

    public void TrainMoveStart()
    {
        foreach (BoxCollider col in colliders)
        {
            col.enabled = true;
        }
    }
}
