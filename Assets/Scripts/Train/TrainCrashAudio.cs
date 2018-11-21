using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrainCrashAudio : MonoBehaviour {

	void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.gameObject.name == "FrontCollideChecker")
        {
            GetComponent<AudioPlayer>().Play("crash");
        }
        
    }
}
