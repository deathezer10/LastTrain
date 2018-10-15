using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorLever : StationaryObject
{
    private GameObject AcceleratorHandle;
    BoxCollider HandleCollider;
    private GameObject PlayerHand;


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
        AcceleratorHandle = HandleCollider.gameObject;
        HandleMaxPosition = AcceleratorHandle.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bDisableLever)
        {
            if (bIsGrabbing)
            {

                if (LastHandPosition < currentHandPosition.z)
                {
                    if (currentHandPosition.z >= HandleMaxPosition)
                    {
                        print("Handle max position reached");
                        bIsGrabbing = false;
                        AcceleratorHandle.transform.SetPositionAndRotation(new Vector3(AcceleratorHandle.transform.position.x,
                        AcceleratorHandle.transform.position.y, HandleMaxPosition), AcceleratorHandle.transform.rotation);
                        return;
                    }
                }

                else if (LastHandPosition > currentHandPosition.z)
                {
                    if (currentHandPosition.z <= HandleMinPosition)
                    {
                        print("Handle min position reached, event should be fired");
                        bIsGrabbing = false;
                        bDisableLever = true;
                        //TODO: Event for accelerator stopped
                    }

                    else
                    {
                        AcceleratorHandle.transform.SetPositionAndRotation(new Vector3(AcceleratorHandle.transform.position.x, AcceleratorHandle.transform.position.y,
                        PlayerHand.transform.position.z), AcceleratorHandle.transform.rotation);
                        LastHandPosition = currentHandPosition.z;
                        
                    }
                }

                
                





            }
        }
    }


    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        bCanGrab = true;
        print("Controller entered");
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
            print("Controller grabbed");
            bIsGrabbing = true;
            LastHandPosition = PlayerHand.transform.position.z;
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
