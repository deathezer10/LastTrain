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
      float newZ =  Mathf.Lerp(SlowestSpeedPosition.z, HighestSpeedPosition.z, normalize01(stationMover.currentSpeed, 0, 20));
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.gameObject.name == "FrontCollideChecker")
        {
            GetComponent<AudioPlayer>().Play("crash");
        }
        
    }

    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }
}
