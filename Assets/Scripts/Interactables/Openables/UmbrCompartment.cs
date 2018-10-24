using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrCompartment : MonoBehaviour
{
    public GameObject visualLockTop, visualLockBottom, physicsLockTop, physicsLockBottom, umbrella;

    bool rotating;
    Quaternion finalRotation = new Quaternion(0, 0.7f, 0, 0.7f);
    
    void Update()
    {
        if (rotating && transform.localRotation != finalRotation)
        {
            transform.Rotate(0f, 60f * Time.deltaTime, 0f);

            if (transform.localRotation.y >= 0.7f)
            {
                rotating = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SawBlade" && other.GetComponentInParent<SawBlade>().IsSpinning())
        {
            OpenCompartment();
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

        umbrella.SetActive(true);
        umbrella.transform.SetParent(null);

        rotating = true;
    }
}
