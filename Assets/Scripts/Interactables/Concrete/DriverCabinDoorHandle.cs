using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverCabinDoorHandle : StationaryObject {

    private PlayerViveController[] foundControllers;
    private GameObject PlayerHand;

    private bool bIsGrabbing = false;
    private bool bDisableDoor = false;
    private bool bCanGrab = false;

    private Vector3 CurrentHandPosition;
    private Vector3 PreviousHandPosition;

	// Update is called once per frame
	void Update () {
		
        if(!bDisableDoor)
        {
            if(bIsGrabbing)
            {
                Vector3 targetDir = PreviousHandPosition - transform.parent.position;
                Vector3 NewtargetDir = CurrentHandPosition - transform.parent.position;
                float angle = Vector3.Angle(targetDir, NewtargetDir);

                Vector3 cross = Vector3.Cross(targetDir, NewtargetDir);

                if (cross.y < 0) angle = -angle;

                /*
                if (angle < 0)
                    if (CurrentYRotation <= MinYRotation)
                    {
                        bIsGrabbing = false;
                        bDisableDoor = true;
                        //TODO here: Activate the functions for engaging the brake/accelerator
                        return;
                    }

                if (angle > 0)
                    if (CurrentYRotation >= MaxYRotation)
                    {
                        bIsGrabbing = false;
                        return;
                    }
                    */

                transform.parent.Rotate(0, -angle, 0);
                PreviousHandPosition = CurrentHandPosition;
            }
        }
	}

    public override void OnControllerEnter(PlayerViveController currentController)
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
            CurrentHandPosition = PlayerHand.transform.position;
        }
    }

    public override void OnGrab()
    {
        if (bCanGrab)
        {
            bIsGrabbing = true;
            PreviousHandPosition = PlayerHand.transform.position;
            CurrentHandPosition = PlayerHand.transform.position;

        }
    }

    public override void OnGrabReleased()
    {
        bIsGrabbing = false;
    }

    public override void OnUse()
    {
        
    }




}

