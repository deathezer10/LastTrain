using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PlayerColliderDeathHandler : MonoBehaviour {

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
            if (!FindObjectOfType<TrainArriver>().HasArrived)
                FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_trainhit");
        }
    }
}
