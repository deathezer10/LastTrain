using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPhysicsParenter : MonoBehaviour {
    private GameObject DoorPhysics;



	// Use this for initialization
	void Start () {
        DoorPhysics = transform.GetChild(1).gameObject;
        transform.gameObject.transform.parent = DoorPhysics.transform;
        DoorPhysics.GetComponent<SphereCollider>().enabled = false;
        DoorPhysics.GetComponent<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
