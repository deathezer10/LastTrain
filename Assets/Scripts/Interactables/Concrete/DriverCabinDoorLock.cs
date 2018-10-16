using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverCabinDoorLock : StationaryObject
{


    public static DriverCabinDoorLock instance;
    private GameObject PlayerHand;

    public static bool bIsUnlocked = false;
    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;

    private Vector3 CurrentHandPosition;
    private Vector3 LastHandPosition;
    private Vector3 HandleMovementDirection = new Vector3(1, 0, 0);
    private float timer;
    private float velocity;
    private float timed;
    private float distance;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!bDisableLever)
        {
            if (bIsGrabbing)
            {
                timer = Time.time;
                Vector3 HandMovementDirection = PlayerHand.transform.position - LastHandPosition;
                distance = Vector3.Distance(PlayerHand.transform.position, LastHandPosition);
                

                HandMovementDirection.Normalize();

                if (AlmostEqual(HandMovementDirection, HandleMovementDirection, 0.40015f))
                {
                    /*
                    if (VectorEndPoint.transform.position.z <= AcceleratorHandle.transform.position.z)
                    {
                        //TODO: EVENT for Setting ACC. to ZERO
                        bIsGrabbing = false;
                        bDisableLever = true;
                        return;
                    }
                    */

                    transform.parent.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position);
                    LastHandPosition = PlayerHand.transform.position;
                    timed = Time.time - timer;
                    return;

                }

                if (AlmostEqual(HandMovementDirection, -HandleMovementDirection, 0.40015f))
                {
                    /*
                    if (HandleDefaultMaxPosition.z >= AcceleratorHandle.transform.position.z)
                    {
                        return;
                    }
                    */

                    transform.parent.position -= HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position);
                    LastHandPosition = PlayerHand.transform.position;
                    timed = Time.time - timer;
                    return;

                }


            }
        }

    }
    void Awake()
    {
        instance = this;
    }


    public static void init()
    {
        bIsUnlocked = true;
    }

    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {

        if(bIsUnlocked)
        {
            bCanGrab = true;
            PlayerHand = currentController.gameObject;
        }
       
    }

    public override void OnControllerExit()
    {
        bCanGrab = false;
    }

    public override void OnControllerStay()
    {
        if (bIsGrabbing)
        {
            CurrentHandPosition = PlayerHand.transform.position;
        }
    }

    public override void OnGrab()
    {
        if (bCanGrab)
        {
            bIsGrabbing = true;
            LastHandPosition = PlayerHand.transform.position;
        }
    }

    public override void OnGrabReleased(bool snapped)
    {
        bIsGrabbing = false;
        velocity = distance / timed;
        print(velocity.ToString("F2"));
        
    }

    public override void OnUse()
    {
    


    }

    private bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

}
