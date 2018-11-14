using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMeter : MonoBehaviour {

    
    private float DefaultRotation;

    [SerializeField]
    private float MaxRotation;

    private StationMover stationMover;

	// Use this for initialization
	void Start () {
        DefaultRotation = transform.localRotation.eulerAngles.y;
        stationMover = FindObjectOfType<StationMover>();
	}
	
	private void Update()
    {
        if(transform.name == "SpeedMeter_Pointer")
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.Lerp(DefaultRotation, MaxRotation, normalize01(stationMover.currentSpeed, 0, 20) ), transform.localRotation.eulerAngles.z);

        else
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.Lerp(DefaultRotation, MaxRotation, normalize01(stationMover.currentSpeed, 0, 4)), transform.localRotation.eulerAngles.z);
        }
    }


    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }
}
