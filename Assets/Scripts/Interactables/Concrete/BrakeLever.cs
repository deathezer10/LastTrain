using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLever : StationaryObject
{
    public static BrakeLever instance;
    private GameObject PlayerHand;
    private BoxCollider DisablePoint;
    private AudioPlayer Audio;
    private StationMover stationmover;

    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bCanGrab = false;
    private bool bStopTrain = false;
    private bool bSlowDownTrain = false;

    private Vector3 HandOffsetStart;
    private Vector3 currentHandPosition;
    private float minXRotation = -0.45f;       //Setting lowest reachable rotation for the lever
    private float maxXRotation;               //Setting the max reachable rotation for the lever
    private float currentXRotation;

    private float PreviousTrainSpeed = 10.0f;
    private float NewTrainSpeed;
    private float rate = 0.3f;
    private float i;

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
        stationmover = FindObjectOfType<StationMover>();
        currentXRotation = transform.rotation.x;
        maxXRotation = currentXRotation;
    }

    // Update is called once per frame
    void Update()
    {

        if(bStopTrain)
        {
            i += Time.deltaTime * rate;
            print(Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i));
            stationmover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i);

        }

        if(bSlowDownTrain)
        {
            i += Time.deltaTime * rate;
            print(Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i));
            stationmover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i);
        }

        if (!bDisableLever)
        {
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
                        Audio.Play();

                        if (AcceleratorLever.IsTaskCompleted())
                        {
                            bStopTrain = true;
                            PreviousTrainSpeed = stationmover.currentSpeed;
                            NewTrainSpeed = 0;
                            i = 0;
                            //This was last lever to be activated train is now stopping? do something
                            return;
                        }

                        else
                        {
                            bSlowDownTrain = true;
                            PreviousTrainSpeed = stationmover.currentSpeed;
                            NewTrainSpeed = 3;
                            stationmover.currentMaxSpeed = 3;
                            i = 0;
                            return;
                        }
                            
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
    }

    public override bool hideControllerOnGrab { get { return false; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        if(DriverCabinDoorLock.bIsUnlocked)
        {
            bCanGrab = true;
            PlayerHand = currentController.gameObject;
        }
       
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

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
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
