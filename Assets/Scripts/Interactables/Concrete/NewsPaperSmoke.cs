using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsPaperSmoke : MonoBehaviour
{
    public GameObject smokeObject, burnoutParticle;

    Color paperTextureColor;

    AudioPlayer burningAudio;
    bool smoking;

    float burnOutTime;

    void Start()
    {
        smokeObject.GetComponent<CapsuleCollider>().enabled = false;
        paperTextureColor = GetComponentInChildren<MeshRenderer>().material.color;
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
        burnOutTime = Time.time + 30f;  // Time until the newspaper is destroyed by the fire
        StartCoroutine(NewspaperBurnEffect());
        burningAudio.Play();
        smokeObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private IEnumerator NewspaperBurnEffect()
    {
        while (burnOutTime >= Time.time)
        {
            paperTextureColor.r -= 0.004f;
            paperTextureColor.b -= 0.004f;
            paperTextureColor.g -= 0.004f;
            yield return new WaitForSeconds(0.2f);
        }

        burningAudio.Stop();
        Instantiate(burnoutParticle, transform.position, transform.rotation, null);
        Destroy(gameObject);
    }
}
