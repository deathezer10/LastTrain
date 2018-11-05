using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainVelocity : MonoBehaviour
{
    private Vector3 previous;

    public float GetVelocity { get; private set; }

    void Update()
    {

        GetVelocity = ((transform.position - previous).magnitude) / Time.deltaTime;
        previous = transform.position;
    }
}
