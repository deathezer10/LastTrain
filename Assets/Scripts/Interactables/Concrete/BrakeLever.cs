using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLever : StationaryObject
{

    private GameObject PlayerHand;
    private BoxCollider DisablePoint;
    private AudioPlayer Audio;
    private TrainSpeedHandler trainSpeedHandler;

    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bCanGrab = false;

    private Vector3 HandOffsetStart;
    private Vector3 currentHandPosition;
    private float minXRotation = -0.45f;       //Setting lowest reachable rotation for the lever
    private float maxXRotation;               //Setting the max reachable rotation for the lever
    private float currentXRotation;
    public bool bBrakeIsStuck = true;

    // Use this for initialization
    void Start()
    {

        currentXRotation = transform.rotation.x;
        maxXRotation = currentXRotation;
        trainSpeedHandler = FindObjectOfType<TrainSpeedHandler>();
        Audio = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bDisableLever) return;
        
            if (bIsGrabbing)
            {
                Vector3 targetDir = HandOffsetStart - transform.position;
                Vector3 NewtargetDir = currentHandPosition - transform.position;
                float angle = Vector3.Angle(targetDir, NewtargetDir);

                Vector3 cross = Vector3.Cross(targetDir, NewtargetDir);
                if (cross.x < 0) angle = -angle;

                if (angle < 0)
                    if (currentXRotation <= minXRotation)
                    {
                        bIsGrabbing = false;
                        bDisableLever = true;
                        Audio.Play("lever");
                        trainSpeedHandler.BrakeStop();
                        return;

                    }


                if (angle > 0)
                    if (currentXRotation >= maxXRotation)
                    {
                        return;
                    }

                transform.Rotate(angle, 0, 0);
                HandOffsetStart = currentHandPosition;
                currentXRotation = transform.rotation.x;
            }
        
    }

    public override bool hideControllerOnGrab { get { return false; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        if (DriverCabinDoorLock.bIsUnlocked && !bBrakeIsStuck)
        {
            bCanGrab = true;
            PlayerHand = currentController.gameObject;
        }
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();

        bCanGrab = false;
    }

    public override void OnControllerStay()
    {
        base.OnControllerStay();

        if (PlayerHand != null)
            currentHandPosition = PlayerHand.transform.position;
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (bCanGrab)
        {
            bIsGrabbing = true;
            HandOffsetStart = PlayerHand.transform.position;
            currentHandPosition = PlayerHand.transform.position;
        }
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        bIsGrabbing = false;
    }
}
