using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePlatform : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;
            GetComponentInChildren<EscapeTunnelDoor>().OpenEscapeDoor();
            Destroy(FindObjectOfType<DummyTrain>().gameObject);
            FindObjectOfType<PlayerVictoryHandler>().PlayerVictory();
        }
    }

}
