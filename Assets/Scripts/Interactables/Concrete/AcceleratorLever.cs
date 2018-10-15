using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorLever : StationaryObject
{
    private GameObject Accelerator;
    private GameObject AcceleratorHandle;
    BoxCollider HandleCollider;
    private GameObject PlayerHand;
    private GameObject VectorBeginPoint;
    private GameObject VectorEndPoint;
    private Vector3 HandleMovementDirection;
    private Vector3 LastHandPosition;

    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;


    private Vector3 currentHandPosition;

    // Use this for initialization
    void Start()
    {
        HandleCollider = GetComponent<BoxCollider>();
        Accelerator = transform.parent.gameObject;
        AcceleratorHandle = HandleCollider.gameObject;
        VectorBeginPoint = Accelerator.transform.GetChild(2).gameObject;
        VectorEndPoint = Accelerator.transform.GetChild(1).gameObject;
        HandleMovementDirection = VectorEndPoint.transform.position - VectorBeginPoint.transform.position;
        HandleMovementDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bDisableLever)
        {
            if (bIsGrabbing)
            {
                Vector3 HandMovementDirection = PlayerHand.transform.position - LastHandPosition;
                HandMovementDirection.Normalize();
                if (AlmostEqual(HandMovementDirection, HandleMovementDirection, 0.00015f))
                {
                    print("Downwards almost equal");
                    AcceleratorHandle.transform.position += HandleMovementDirection * Vector3.Distance(LastHandPosition,PlayerHand.transform.position);
                    LastHandPosition = PlayerHand.transform.position;
                    return;

                }
               
                if(AlmostEqual(HandMovementDirection,-HandleMovementDirection,0.00015f))
                {
                    print("Upwards almost equal");
                    AcceleratorHandle.transform.position -= HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position);
                    LastHandPosition = PlayerHand.transform.position;
                    return;

                }



                




            }
        }
    }


    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        bCanGrab = true;
        PlayerHand = currentController.gameObject;
    }

    public override void OnControllerExit()
    {
        bCanGrab = false;
    }

    public override void OnControllerStay()
    {
        if (bIsGrabbing)
        {
            currentHandPosition = PlayerHand.transform.position;
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
