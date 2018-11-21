using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrainCrashAudio : MonoBehaviour {

    public StationMover stationMover;
    private Vector3 HighestSpeedPosition;
    private Vector3 SlowestSpeedPosition;

	void Start()
    {
        HighestSpeedPosition = transform.position;
        SlowestSpeedPosition = transform.Find("slowest").transform.position;
        stationMover = FindObjectOfType<StationMover>();
    }

    void Update()
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
