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

        colliders[0].size = new Vector3(0.9f, 0.7f, 36f);

        wallCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainjump");
        }
        else if (other.tag == "Bomb")
        {
            other.gameObject.GetComponent<Bomb>().ThrownOut();

            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, -2f * stationMover.currentSpeed), ForceMode.Impulse);
        }
        else if (other.tag == "Doll")
        {
            other.gameObject.GetComponent<Doll>().OnThrownOut();

            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, -2f * stationMover.currentSpeed), ForceMode.Impulse);
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<IInteractable>() != null)
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 3f, -2f * stationMover.currentSpeed), ForceMode.Impulse);
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

    public void TrainMoveStop()
    {
        foreach (BoxCollider col in colliders)
        {
            col.enabled = false;
        }

        wallCollider.enabled = false;
    }
}
