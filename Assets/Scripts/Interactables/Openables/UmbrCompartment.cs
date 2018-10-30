using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrCompartment : MonoBehaviour
{
    public GameObject visualLockBottom, visualLockTop, physicsLockBottom, physicsLockTop, umbrella, umbrellaVisual;

    bool rotating;
    //AudioPlayer lockBreakAudio;
    Quaternion finalRotation = new Quaternion(0, 0.7f, 0, 0.7f);

    private void Start()
    {
        //lockBreakAudio = GetComponent<AudioPlayer>();
    }

    void Update()
    {
        if (rotating && transform.localRotation != finalRotation)
        {
            transform.Rotate(0f, 80f * Time.deltaTime, 0f);

            if (transform.localRotation.y >= 0.7f)
            {
                rotating = false;
                umbrellaVisual.SetActive(false);
                umbrella.SetActive(true);
                umbrella.transform.SetParent(null);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SawBlade" && other.GetComponentInParent<SawBlade>().IsSpinning())
        {
            OpenCompartment();
            //lockBreakAudio.Play();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    public void OpenCompartment()
    {
        visualLockBottom.SetActive(false);
        visualLockTop.SetActive(false);

        physicsLockBottom.SetActive(true);
        physicsLockBottom.transform.parent = null;

        physicsLockTop.SetActive(true);
        physicsLockTop.transform.parent = null;

        rotating = true;
    }
}
