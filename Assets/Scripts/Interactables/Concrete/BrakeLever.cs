using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLever : StationaryObject
{
    public static BrakeLever instance;
    private GameObject PlayerHand;
    private PlayerViveController[] foundControllers;
    private BoxCollider LeverTip;
    private BoxCollider DisablePoint;
    private AudioPlayer Audio;

    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bCanGrab = false;

    private Vector3 HandOffsetStart;
    private Vector3 currentHandPosition;
    private float minXRotation = -0.45f;       //Setting lowest reachable rotation for the lever
    private float maxXRotation;               //Setting the max reachable rotation for the lever
    private float currentXRotation;
   

    public static bool IsTaskCompleted()
    {
        return instance.bDisableLever;
    }

    void Awake()
    {
        instance = this;
        Audio = GetComponent<AudioPlayer>();
    }

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
        if (!bDisableLever)
        {
            if (bIsGrabbing)
            {
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
                        Audio.Play();

                        if (AcceleratorLever.IsTaskCompleted())
                        {
                            //This was last lever to be activated train is now stopping? do something
                            return;
                        }

                        else
                            return; //Player still has to do the accelerator lever
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
    }

    public override void OnControllerExit()
    {
        bCanGrab = false;
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
