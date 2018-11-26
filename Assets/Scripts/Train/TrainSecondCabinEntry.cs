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

            FindObjectOfType<SmokeAlarm>().StopSmokeAlarm("cabin2");

            AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
            AnnouncementManager.Instance.PlayAnnouncement3D("secondCabin_entry", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);

            Destroy(gameObject);
        }

      

    }
}
