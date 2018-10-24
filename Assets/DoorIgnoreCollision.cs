using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIgnoreCollision : MonoBehaviour
{

    private BoxCollider Collider;
    private bool bIsColliding;

    // Use this for initialization
    void Start()
    {
        Collider = transform.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bIsColliding)
        {
            Collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Collider.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            Collider.gameObject.GetComponent<Rigidbody>().Sleep();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        print("Colission start");
        bIsColliding = true;
        Collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Collider.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Collider.gameObject.GetComponent<Rigidbody>().Sleep();

    }

    void OnCollisionLeave(Collision other)
    {
        bIsColliding = false;
    }

}
