using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyHandlePanel : StationaryObject
{

    private GameObject PlayerHand;

    private bool bIsGrabbing = false;
    private bool bCanGrab = false;
    public bool bIsLocked = false;
    public bool bIsOpened = false;
    private Vector3 CurrentHandPosition;
    private Vector3 PreviousHandPosition;

    private float maxYRotation;
    private float DefaultYRotation;



    void Start()
    {
        DefaultYRotation = transform.parent.rotation.eulerAngles.y;
        maxYRotation = transform.parent.rotation.eulerAngles.y + 140;
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


            if (transform.parent.rotation.eulerAngles.y >= maxYRotation - 35 && transform.parent.rotation.eulerAngles.y <= maxYRotation)
            {
                bIsOpened = true;
            }

            else bIsOpened = false;

            if (angle < 0)
                if (transform.parent.rotation.eulerAngles.y <= DefaultYRotation || (transform.parent.rotation.eulerAngles.y <= 360 && transform.parent.rotation.eulerAngles.y >= (DefaultYRotation + 151)))
                {
                    return;
                }

            if (angle > 0)
                if (transform.parent.rotation.eulerAngles.y >= maxYRotation && transform.parent.rotation.eulerAngles.y <= (maxYRotation + 20))
                {
                    return;
                }


            transform.parent.Rotate(0, angle, 0);
            PreviousHandPosition = CurrentHandPosition;
        }

    }

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        if (!bIsLocked)
            bCanGrab = true;
        PlayerHand = currentController.gameObject;
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();

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
        base.OnGrab();
        
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


}
