using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIgnoreCollision : MonoBehaviour
{
    private Rigidbody rbDoor;
    public bool bIsColliding;

    // Use this for initialization
    void Start()
    {
        rbDoor = transform.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bIsColliding)
        {
            rbDoor.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        print("collision enter");
        bIsColliding = true;
        rbDoor.isKinematic = true;
        rbDoor.velocity = Vector3.zero;
    }

    void OnCollisionExit(Collision other)
    {
        print("Collision exit");
        bIsColliding = false;
        rbDoor.isKinematic = false;
        rbDoor.velocity = Vector3.zero;
    }

}
