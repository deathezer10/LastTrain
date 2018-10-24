using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIgnoreCollision : MonoBehaviour
{

    private BoxCollider Collider;
    public bool bIsColliding;

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
        bIsColliding = true;
    }

    void OnCollisionLeave(Collision other)
    {
        bIsColliding = false;
    }

}
