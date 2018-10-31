using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PlayerColliderDeathHandler : MonoBehaviour , IShootable
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FullMergedTrain")
        {
            if (FindObjectOfType<TrainVelocity>().GetVelocity > 2)
                FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainhit");
        }
    }

    public void OnShot(Revolver revolver)
    {
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_gunsuicide");
    }
}
