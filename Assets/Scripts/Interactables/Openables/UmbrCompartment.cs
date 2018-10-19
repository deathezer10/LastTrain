using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrCompartment : MonoBehaviour
{
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

    public void OpenCompartment()
    {
        rotating = true;
    }
}
