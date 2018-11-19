using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPhysicsParenter : MonoBehaviour {
    private GameObject DoorPhysics;
    private GameObject DoorHandle;

	// Use this for initialization
	void Start () {
        DoorPhysics = transform.GetChild(1).gameObject;
        DoorHandle = transform.GetChild(0).gameObject;
        transform.gameObject.transform.parent = DoorPhysics.transform;
        DoorPhysics.transform.parent = transform.parent;
        this.transform.parent = DoorPhysics.transform;
        DoorHandle.transform.parent = DoorPhysics.transform;
	}	
}
