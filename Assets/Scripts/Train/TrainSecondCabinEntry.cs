using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSecondCabinEntry : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;

            FindObjectOfType<TrainBetweenCabinDoors>().CloseBetweenDoors();

            FindObjectOfType<SmokeAlarm>().StopSmokeAlarm("Cabin2");

            AnnouncementManager.Instance.PlayAnnouncement3D("secondCabin_entry", AnnouncementManager.AnnounceType.Queue, 0f);

            Destroy(gameObject);
        }
    }
}
