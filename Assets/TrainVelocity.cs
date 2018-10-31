using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainVelocity : MonoBehaviour {
    private Vector3 previous;
    private float velocity;
    public float GetVelocity
        {
        get { return velocity; }
        }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        velocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;

        // print(velocity);
    }
}
