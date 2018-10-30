using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsPaperSmoke : MonoBehaviour
{
    public GameObject smokeObject;

    AudioPlayer burningAudio;
    bool smoking;

    void Start()
    {
        smokeObject.GetComponent<CapsuleCollider>().enabled = false;
        burningAudio = GetComponent<AudioPlayer>();
    }
    
    void Update()
    {
        if (smoking)
        {
            smokeObject.transform.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (smoking)
        {
            if (other.tag == "SmokeDetector")
            {
                other.GetComponent<SmokeDetector>().DetectingSmoke(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (smoking)
        {
            if (other.tag == "SmokeDetector")
            {
                other.GetComponent<SmokeDetector>().DetectingSmoke(false);
            }
        }
    }

    public void SmokingStart()
    {
        smoking = true;
        burningAudio.Play();
        smokeObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
