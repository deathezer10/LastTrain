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
        if (other.gameObject.name == "FullMergedTrain")
        {
            if (FindObjectOfType<TrainVelocity>().GetVelocity > 2 && !FindObjectOfType<TrainArriver>().HasArrived)
               playerDeathHandler.KillPlayer("death_trainhit");
        }

        
        //if(other.gameObject.name == "PlayerDeathCollider" && FindObjectOfType<SawBlade>().IsSpinning())
        //{
        //    playerDeathHandler.KillPlayer("death_sawblade");
        //}
    }

    public void OnShot(Revolver revolver)
    {
        playerDeathHandler.KillPlayer("death_gunsuicide");
    }
}
