using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainEscapeHandler : MonoBehaviour
{
    public BoxCollider wallCollider;

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

        wallCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exited the train, train was moving at a speed of " + stationMover.currentSpeed);

            if (stationMover.currentSpeed <= 2f)
            {
                FindObjectOfType<PlayerVictoryHandler>().PlayerVictory();
            }
            else
            {
                FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainjump");
            }
        }
        else if (other.tag == "Bomb")
        {
            other.gameObject.GetComponent<Bomb>().ThrownOut();

            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, -15f), ForceMode.Impulse);
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.layer != 11)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, -15f), ForceMode.Impulse);
        }

    }

    public void TrainMoveStart()
    {
        foreach (BoxCollider col in colliders)
        {
            col.enabled = true;
        }

        wallCollider.enabled = true;
    }
}
