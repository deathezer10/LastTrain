using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrainCrashAudio : MonoBehaviour {

    public StationMover stationMover;
    private Vector3 HighestSpeedPosition;
    private Vector3 SlowestSpeedPosition;
    public bool bTrainStopped = false;
    private Vector3 previous;
    private GameObject dummyTrain;
    public float GetVelocity { get; private set; }

    void Start()
    {
        HighestSpeedPosition = transform.localPosition;
        SlowestSpeedPosition = transform.Find("slowest").transform.localPosition;
        stationMover = FindObjectOfType<StationMover>();
        dummyTrain = FindObjectOfType<DummyTrain>().gameObject;
       
    }

  


    void Update()
    {
        if(stationMover.currentSpeed == 0)
        {
            bTrainStopped = true;
        }


        GetVelocity = ((dummyTrain.transform.position - previous).magnitude) / Time.deltaTime;
        previous = dummyTrain.transform.position;
        if (!bTrainStopped)
        {
            float newZ = Mathf.Lerp(SlowestSpeedPosition.z, HighestSpeedPosition.z, normalize01(stationMover.currentSpeed, 0, 20));
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZ);
        }

        else
        {
            print(GetVelocity);
            float newZ = Mathf.Lerp(SlowestSpeedPosition.z, HighestSpeedPosition.z, normalize01(GetVelocity, 0, 20));
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZ);
        }



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
