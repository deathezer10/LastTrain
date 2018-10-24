using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIgnoreCollision : MonoBehaviour
{
    private Rigidbody rbDoor;
    public bool bIsColliding;
    private BoxCollider collider;
    private float sqrMinimumExtent;
    private float movementSqrMagnitude;
    private float minimumExtent;
    private Vector3 movementNextStep;
    private float movementMagnitude;
    private RaycastHit hitInfo;
    Vector3 currentPosition = new Vector3();
    Vector3 newPosition = new Vector3();
    Vector3 nextPosition = new Vector3();

    void Awake()
    {
        collider = transform.GetComponent<BoxCollider>();
        rbDoor = transform.gameObject.GetComponent<Rigidbody>();
        minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z);
        //partialExtent = minimumExtent * (1.0f - skinWidth); 
        sqrMinimumExtent = minimumExtent * minimumExtent;
        rbDoor.velocity = Vector3.zero;
        rbDoor.angularVelocity = Vector3.zero;

        
        
    }
    void FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        currentPosition = rbDoor.position;
        newPosition.x = currentPosition.x + rbDoor.velocity.x * Time.deltaTime;
        newPosition.y = currentPosition.y + rbDoor.velocity.y * Time.deltaTime - 0.5f * -9.81f * Time.deltaTime * Time.deltaTime;
        newPosition.z = currentPosition.z + rbDoor.velocity.z * Time.deltaTime;
        //newVelocity.y = oldVelocity.y - gravity * timeDelta;
        nextPosition.x = newPosition.x + rbDoor.velocity.x * Time.deltaTime;
        nextPosition.y = newPosition.y + rbDoor.velocity.y * Time.deltaTime - 0.5f * -9.81f * Time.deltaTime * Time.deltaTime;
        nextPosition.z = newPosition.z + rbDoor.velocity.z * Time.deltaTime;
        //nextVelocity.y = newVelocity.y - gravity * timeDelta;

        movementNextStep = nextPosition - newPosition;
        movementSqrMagnitude = (nextPosition - currentPosition).sqrMagnitude;

        if (movementSqrMagnitude > sqrMinimumExtent)
        {
            movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);

            //check for obstructions we will have hit
            //Debug.DrawLine (currentPosition, nextPosition);
            if (Physics.Raycast(currentPosition, movementNextStep, out hitInfo, movementMagnitude))
            {
                CustomOnCollisionEnter(hitInfo.collider);
            }
        }
    }

    void update()
    {
        if(bIsColliding)
        {
            rbDoor.velocity = Vector3.zero;
        }
    }


    //For visual affects only. Actual collisions are calculated on the server.
    void CustomOnCollisionEnter(Collider colliderInfo)
    {
        print("CustomOnCollision");
        bIsColliding = true;
        rbDoor.isKinematic = true;
        rbDoor.velocity = Vector3.zero;

    }

    void OnCollisionExit(Collider collider)
    {
        bIsColliding = false;
        rbDoor.isKinematic = false;
        rbDoor.velocity = Vector3.zero;
    }


}
