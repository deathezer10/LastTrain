﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyHandlePanel : StationaryObject {

    private GameObject PlayerHand;
  
    private bool bIsGrabbing = false;
    private bool bCanGrab = false;
    private bool bIsLocked = false;

    private Vector3 CurrentHandPosition;
    private Vector3 PreviousHandPosition;

    private float maxYRotation;
    private float DefaultYRotation;
  


    void Start()
    {
        DefaultYRotation = transform.rotation.eulerAngles.y;
        maxYRotation = transform.rotation.eulerAngles.y + 120;
    }

    // Update is called once per frame
    void Update()
    {

        if (bIsGrabbing)
        {
            Vector3 targetDir = PreviousHandPosition - transform.position;
            Vector3 NewtargetDir = CurrentHandPosition - transform.position;
            float angle = Vector3.Angle(targetDir, NewtargetDir);

            Vector3 cross = Vector3.Cross(targetDir, NewtargetDir);

            if (cross.y < 0) angle = -angle;


            if (angle < 0)
                if (transform.rotation.eulerAngles.y <= DefaultYRotation || (transform.rotation.eulerAngles.y <= 360 && transform.rotation.eulerAngles.y >= (DefaultYRotation + 150)))
                {
                    return;
                }

            if (angle > 0)
                if (transform.rotation.eulerAngles.y >= maxYRotation && transform.rotation.eulerAngles.y <= (maxYRotation + 20))
                {
                    return;
                }


            transform.Rotate(0, angle, 0);
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
