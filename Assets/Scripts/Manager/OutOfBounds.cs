using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainjump");
        }
        else if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.gameObject.SetActive(false);
        }
    }

}
