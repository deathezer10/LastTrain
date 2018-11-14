using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PlayerColliderDeathHandler : MonoBehaviour , IShootable
{
    PlayerDeathHandler playerDeathHandler;

    void Start () {
        playerDeathHandler = FindObjectOfType<PlayerDeathHandler>();
    }
	
    private void OnTriggerEnter(Collider other)
    {
        // This block handled in TrainEscapeHandler, for now.

        /*
        if (this.gameObject.name == "CameraFollower")
        if (other.gameObject.name == "tunnel_bot")
            {
                if(FindObjectOfType<StationMover>().currentSpeed > 1.5)
                {
                    playerDeathHandler.KillPlayer("death_trainjump");
                }
            }
        */

        if (this.gameObject.name == "CameraFollower") return;
        if (other.gameObject.name == "Banana")
        {
            other.GetComponent<Banana>().BiteBanana();
        }

        if (this.gameObject.name == "CameraFollower") return;
        if (other.gameObject.name == "Shell")
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
