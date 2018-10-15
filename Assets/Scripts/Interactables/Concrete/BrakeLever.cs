using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLever : StationaryObject
{
    private GameObject PlayerHand;
    private PlayerViveController[] foundControllers;
    private BoxCollider LeverTip;

    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bCanGrab = false;

    private Vector3 HandOffsetStart;
    private Transform LastHandLocation;
    Vector3 currentHandPosition;

   // private float MaxHandReach = 10.0f;              //Adjust reach before player lets go of the lever
    private float minZRotation = -0.80f;              //Setting lowest reachable rotation for the lever
    private float maxZRotation = 0.80f;               //Setting the max reachable rotation for the lever
    private float currentZRotation = 0.0f;

    
    

   

    // Use this for initialization
    void Start()
    {
        foundControllers = FindObjectsOfType<PlayerViveController>();
        LeverTip = GetComponent<BoxCollider>();
        currentZRotation = transform.parent.rotation.z;

    }

    // Update is called once per frame
    void Update()
    {
        if (!bDisableLever)
        {

            if (bIsGrabbing)
            {

                //if (MaxHandReach < Vector3.Distance(LeverTip.transform.position, PlayerHand.transform.position))
                //{
                //    Debug.Log("hello world");
                //    bIsGrabbing = false;
                //    bCanGrab = false;
                //    return;
                //}
                //else

                Vector3 targetDir = HandOffsetStart - transform.parent.position;
                Vector3 NewtargetDir = currentHandPosition - transform.parent.position;
                float angle = Vector3.Angle(targetDir, NewtargetDir);

                Vector3 cross = Vector3.Cross(targetDir, NewtargetDir);

                if (cross.z < 0) angle = -angle;

                if (angle < 0)
                    if (currentZRotation <= minZRotation)
                    {
                        bIsGrabbing = false;
                        bDisableLever = true;
                        //TODO here: Activate the functions for engaging the brake/accelerator
                        return;
                    }

                if (angle > 0)
                    if (currentZRotation >= maxZRotation)
                    {
                        bIsGrabbing = false;
                        return;
                    }
                transform.parent.Rotate(0, 0, -angle);
                HandOffsetStart = currentHandPosition;
                currentZRotation = transform.parent.rotation.z;
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

    public override void OnUse()
    {

    }

    public override void OnGrab()
    {
        if (bCanGrab)
        {
            bIsGrabbing = true;
            HandOffsetStart = PlayerHand.transform.position;
        }
    }

    public override void OnGrabReleased(bool snapped)
    {
        bIsGrabbing = false;
    }

}
