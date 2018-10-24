using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIgnoreCollision : MonoBehaviour
{

    private BoxCollider Collider;


    // Use this for initialization
    void Start()
    {
        Collider = transform.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        Collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Collider.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Collider.gameObject.GetComponent<Rigidbody>().Sleep();

    }
}
