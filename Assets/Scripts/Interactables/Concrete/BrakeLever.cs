using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLever : StationaryObject
{
    private GameObject PlayerHand;
    private PlayerViveController[] foundControllers;
    private BoxCollider LeverTip;
    private BoxCollider DisablePoint;

    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bCanGrab = false;

    private Vector3 HandOffsetStart;
    private Transform LastHandLocation;
    Vector3 currentHandPosition;

    // private float MaxHandReach = 10.0f;              //Adjust reach before player lets go of the lever
    private float minXRotation = -0.57f;              //Setting lowest reachable rotation for the lever
    private float maxXRotation;               //Setting the max reachable rotation for the lever
    private float currentXRotation;






    // Use this for initialization
    void Start()
    {
        foundControllers = FindObjectsOfType<PlayerViveController>();
        LeverTip = GetComponent<BoxCollider>();
        currentXRotation = transform.parent.rotation.x;
        maxXRotation = currentXRotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        print(currentXRotation);
       
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

                if (cross.x < 0) angle = -angle;
               


                if (angle < 0)
                    if (currentXRotation <= minXRotation)
                    {
                        bIsGrabbing = false;
                        bDisableLever = true;
                        //TODO here: Activate the functions for engaging the brake/accelerator
                        return;
                    }
                    
                
                if (angle > 0)
                    if (currentXRotation >= maxXRotation)
                    {
                        return;
                    }
                    

                transform.parent.Rotate(angle, 0, 0);
                HandOffsetStart = currentHandPosition;
                currentXRotation = transform.parent.rotation.x;
            }
        }
    }

    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        bCanGrab = true;
        PlayerHand = currentController.gameObject;
        Debug.Log("Enter");
    }

    public override void OnControllerExit()
    {
        bCanGrab = false;
        Debug.Log("Exit");
    }

    public override void OnControllerStay()
    {
        currentHandPosition = PlayerHand.transform.position;
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
            currentHandPosition = PlayerHand.transform.position;
        }
       
    }

    public override void OnGrabReleased()
    {
        bIsGrabbing = false;
    }

}
