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
            PlayerViveController.GetControllerThatHolds(other.gameObject)?.DetachCurrentObject(true);

            other.gameObject.GetComponent<Bomb>().ThrownOut();
            PushObjectAway(other.gameObject);
        }
        else if (other.tag == "Doll")
        {
            PlayerViveController.GetControllerThatHolds(other.gameObject)?.DetachCurrentObject(true);

            other.gameObject.GetComponent<Doll>().OnThrownOut();
            PushObjectAway(other.gameObject);
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.GetComponent<IInteractable>() != null)
        {
            PlayerViveController.GetControllerThatHolds(other.gameObject)?.DetachCurrentObject(true);
            PushObjectAway(other.gameObject);
        }

    }

    private void PushObjectAway(GameObject obj)
    {

        // Disable all colliders for self
        foreach (Collider col in obj.GetComponents<Collider>())
        {
            col.enabled = false;
        }

        var childrens = obj.transform.GetAllChild();

        // Disable all colliders for childrens
        foreach (var child in childrens)
        {
            foreach (Collider col in child.GetComponents<Collider>())
            {
                col.enabled = false;
            }
        }

        var rb = obj.GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.detectCollisions = false;
        rb.AddForce(new Vector3(0, 0, -2f * stationMover.currentSpeed), ForceMode.Impulse);

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
