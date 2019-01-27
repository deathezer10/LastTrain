using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCallManager : MonoBehaviour
{
    [SerializeField]
    private CheckpointManager _checkpointManager = null;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;

            _checkpointManager.CheckpointUpdate(0);

            FindObjectOfType<TrainArriver>().CallTheTrain();

            AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
            AnnouncementManager.Instance.PlayAnnouncement3D("platform_entry", AnnouncementManager.AnnounceType.Queue, 0f);
        }
    }
}
