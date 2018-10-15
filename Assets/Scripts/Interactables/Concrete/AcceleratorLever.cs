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
    private Vector3 HandleMovementVector;

    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;

    private float HandleMaxPosition;
    private float HandleMinPosition = 0.4f;
    private float LastHandPosition;
    private Vector3 currentHandPosition;

    // Use this for initialization
    void Start()
    {
        HandleCollider = GetComponent<BoxCollider>();
        Accelerator = HandleCollider.gameObject;
        AcceleratorHandle = Accelerator.transform.GetChild(0).gameObject;
        VectorBeginPoint = Accelerator.transform.GetChild(2).gameObject;
        VectorEndPoint = Accelerator.transform.GetChild(1).gameObject;
        HandleMovementVector = VectorEndPoint.transform.position - VectorBeginPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bDisableLever)
        {
            if (bIsGrabbing)
            {
                
                
                

                
                





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

        }
    }

    public override void OnGrabReleased(bool snapped)
    {
        bIsGrabbing = false;
    }

    public override void OnUse()
    {

    }




}
