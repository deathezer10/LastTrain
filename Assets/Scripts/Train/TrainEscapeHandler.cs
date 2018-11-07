using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEscapeHandler : MonoBehaviour
{
    bool trainHasStopped;

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (trainHasStopped)
            {
                FindObjectOfType<PlayerVictoryHandler>().PlayerVictory();
            }
            else
            {
                FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainjump");
            }
        }
    }

    public void TrainMoveStart()
    {
        trainHasStopped = false;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void TrainMoveStop()
    {
        trainHasStopped = true;
    }
}
