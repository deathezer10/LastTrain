using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsPaperSmoke : MonoBehaviour
{
    public GameObject smokeObject;

    bool smoking;

    void Start()
    {
        smokeObject.GetComponent<CapsuleCollider>().enabled = false;
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
        smokeObject.GetComponent<CapsuleCollider>().enabled = true;
    }
}
