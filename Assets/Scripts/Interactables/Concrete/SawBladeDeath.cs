using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBladeDeath : MonoBehaviour {
    SawBlade sawblade;
    PlayerDeathHandler playerdeathHandler;
	// Use this for initialization
	void Start () {
        sawblade = FindObjectOfType<SawBlade>();
        playerdeathHandler = FindObjectOfType<PlayerDeathHandler>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.name == "PlayerHeadCollision" && sawblade.IsSpinning())
        {
            playerdeathHandler.KillPlayer("death_sawblade");
        }

        else if(other.name == "Bomb" && sawblade.IsSpinning())
        {
            FindObjectOfType<Bomb>().TimerTimeOut();  
        }
    }

}
