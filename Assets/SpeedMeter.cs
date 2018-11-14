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
        DefaultRotation = transform.rotation.eulerAngles.z;
        stationMover = FindObjectOfType<StationMover>();
	}
	
	private void Update()
    {
        //temp pivot therefore parent
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
            Mathf.Lerp(DefaultRotation, MaxRotation, normalize01(stationMover.currentSpeed, 0, 20))));
        
    }


    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }
}
