using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PlayerColliderDeathHandler : MonoBehaviour , IShootable
{
    PlayerDeathHandler playerDeathHandler;
    // Use this for initialization
    void Start () {
        playerDeathHandler = FindObjectOfType<PlayerDeathHandler>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (this.gameObject.name == "CameraFollower")
        if (other.gameObject.name == "tunnel_bot")
            {
                if(FindObjectOfType<StationMover>().currentSpeed > 1.5)
                {
                    playerDeathHandler.KillPlayer("death_trainjump");
                }
            }

        if (this.gameObject.name == "CameraFollower") return;
        if (other.gameObject.name == "FullMergedTrain")
        {
            if (FindObjectOfType<TrainVelocity>().GetVelocity > 2 && !FindObjectOfType<TrainArriver>().HasArrived)
               playerDeathHandler.KillPlayer("death_trainhit");
        }

    }

    public void OnShot(Revolver revolver)
    {
        if (this.gameObject.name == "CameraFollower") return;
        playerDeathHandler.KillPlayer("death_gunsuicide");
    }
}
