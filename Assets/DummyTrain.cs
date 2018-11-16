using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTrain : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HonkHorn()
    {
        GetComponent<AudioPlayer>().Play();
    }
   
}
