using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverCabinElectricBoxHandle : StationaryObject
{


    private GameObject PlayerHand;

    private bool bIsGrabbing = false;
    private bool bDisableDoor = false;
    private bool bCanGrab = false;
    private bool bIsLocked = true;
    private int ScrewCount = 2;
    private Vector3 CurrentHandPosition;
    private Vector3 PreviousHandPosition;

    private float maxYRotation;
    private float DefaultYRotation;

    void Start()
    {
        Screw.OnLoose += UnScrewed;
        DefaultYRotation = transform.rotation.eulerAngles.y;
        maxYRotation = transform.rotation.eulerAngles.y + 120;
    }

    // Update is called once per frame
    void Update()
    {

            if (bIsGrabbing)
            {
                Vector3 targetDir = PreviousHandPosition - transform.parent.position;
                Vector3 NewtargetDir = CurrentHandPosition - transform.parent.position;
                float angle = Vector3.Angle(targetDir, NewtargetDir);

                Vector3 cross = Vector3.Cross(targetDir, NewtargetDir);

                if (cross.y < 0) angle = -angle;

                
                if (angle < 0)
                    if (transform.rotation.eulerAngles.y <= DefaultYRotation)
                    {
                        bIsGrabbing = false;
                        return;
                    }

                if (angle > 0)
                    if (transform.rotation.eulerAngles.y >= maxYRotation)
                    {
                        bIsGrabbing = false;
                        return;
                    }
                    

                transform.parent.Rotate(0, angle, 0);
                PreviousHandPosition = CurrentHandPosition;
            }
        
    }

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        if (!bIsLocked)
            bCanGrab = true;
        PlayerHand = currentController.gameObject;
    }

    public override void OnControllerExit()
    {
        bCanGrab = false;
        bIsGrabbing = false;
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

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

    private void UnScrewed(string _object)
    {
        if(_object == "Electric")
        {
            ScrewCount -= 1;
            if (ScrewCount == 0)
            {
                bIsLocked = false;
            }
        }
    }
}

