using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsPaperSmoke : MonoBehaviour
{
    public GameObject smokeObject, burnoutParticle;

    Color paperTextureColor;
    Material paperMaterial;

    AudioPlayer burningAudio;
    bool smoking;

    float burnOutTime;

    void Start()
    {
        smokeObject.GetComponent<CapsuleCollider>().enabled = false;
        paperTextureColor = new Color(1f, 1f, 1f);
        paperMaterial= GetComponentInChildren<MeshRenderer>().material;
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
        if (!smoking)
        {
            burnOutTime = Time.time + 25f;  // Time until the newspaper is destroyed by the fire
            StartCoroutine(NewspaperBurnEffect());
            burningAudio.Play();
        }

        smoking = true;
        smokeObject.GetComponent<CapsuleCollider>().enabled = true;
    }

    private IEnumerator NewspaperBurnEffect()
    {
        while (burnOutTime >= Time.time)
        {
            paperTextureColor.r -= 0.004f;
            paperTextureColor.b -= 0.004f;
            paperTextureColor.g -= 0.004f;
            paperMaterial.color = paperTextureColor;
            yield return new WaitForSeconds(0.1f);
        }

        burningAudio.Stop();
        Instantiate(burnoutParticle, transform.position, transform.rotation, null);
        PlayerViveController.GetControllerThatHolds(gameObject)?.DetachCurrentObject(false);
        Destroy(gameObject);
    }
}
