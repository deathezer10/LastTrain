using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtghsrCompartment : MonoBehaviour
{
    public GameObject visualExtinguisher, physicsExtinguisher;

    bool rotating;
    Quaternion finalRotation = new Quaternion(0.7f, 0, 0, 0.7f);
    
    void Update()
    {
        if (rotating && transform.localRotation != finalRotation)
        {
            transform.Rotate(80f * Time.deltaTime, 0f, 0f);

            if (transform.localRotation.x >= 0.7f)
            {
                rotating = false;
                visualExtinguisher.SetActive(false);
                physicsExtinguisher.SetActive(true);
            }
        }
    }

    public void OpenExCompartment()
    {
        rotating = true;
        GetComponent<CapsuleCollider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {
            OpenExCompartment();
        }
    }
}
