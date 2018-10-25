using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPhysicsParenter : MonoBehaviour {
    private GameObject DoorPhysics;
    private GameObject DoorHandle;



	// Use this for initialization
	void Start () {
        DoorPhysics = transform.GetChild(1).gameObject;
        transform.gameObject.transform.parent = DoorPhysics.transform;
        DoorHandle = transform.GetChild(0).gameObject;
        DoorHandle.transform.parent = DoorPhysics.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
