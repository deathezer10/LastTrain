using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollSmoke : MonoBehaviour
{
    AudioPlayer burnAudio;

    private void Start()
    {
        burnAudio = GetComponent<AudioPlayer>();
    }

    private void OnEnable()
    {
        //burnAudio.Play();
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SmokeDetector")
        {
            other.GetComponent<SmokeDetector>().DetectingSmoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SmokeDetector")
        {
            other.GetComponent<SmokeDetector>().DetectingSmoke(false);
        }
    }

}
