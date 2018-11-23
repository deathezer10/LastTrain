using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorLever : StationaryObject
{
    private TrainSpeedHandler trainSpeedHandler;
    public static AcceleratorLever instance;
    private GameObject Accelerator;
    private GameObject AcceleratorHandle;
    private BoxCollider HandleCollider;
    private GameObject PlayerHand;
    private GameObject VectorBeginPoint;
    private GameObject VectorEndPoint;

    private Vector3 HandleMovementDirection;
    private Vector3 LastHandPosition;

    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bOnce = true;
    private float NewTrainSpeed;

    private AudioPlayer Audio;

    //Static function for brakelever to check if this AcceleratorLever is engaged.
    public static bool IsTaskCompleted()
    {
        return instance.bDisableLever;
    }

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        HandleCollider = GetComponent<BoxCollider>();
        Accelerator = gameObject;
        AcceleratorHandle = HandleCollider.gameObject;
        VectorBeginPoint = Accelerator.transform.GetChild(1).gameObject;
        VectorEndPoint = Accelerator.transform.GetChild(0).gameObject;

        // Unparent the child
        VectorBeginPoint.transform.parent = transform.parent;
        VectorEndPoint.transform.parent = transform.parent;

        HandleMovementDirection = VectorEndPoint.transform.position - VectorBeginPoint.transform.position;
        HandleMovementDirection.Normalize(); //The direction where Acceleratorhandle can be moved forth and back.

        trainSpeedHandler = FindObjectOfType<TrainSpeedHandler>();
        Audio = GetComponent<AudioPlayer>();
    }

    void Update()
    {

        if (bIsGrabbing) //Player is grabbing the acceleratorhandle
        {
            if (Vector3.Distance(PlayerHand.transform.position, AcceleratorHandle.transform.position) > 0.2)
            {
                bIsGrabbing = false;
            }


            Vector3 HandMovementDirection = PlayerHand.transform.position - LastHandPosition; //We get the small movement vector of player's hand
            HandMovementDirection.Normalize();

            if (!bOnce)
            {
                if (AlmostEqual(HandMovementDirection, HandleMovementDirection, 0.60015f))
                {
                    if (VectorEndPoint.transform.position.z >= AcceleratorHandle.transform.position.z)
                    {
                        return;
                    }

                    AcceleratorHandle.transform.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Move the handle
                    LastHandPosition = PlayerHand.transform.position;
                    return;
                }

                else if (AlmostEqual(HandMovementDirection, -HandleMovementDirection, 0.60015f))
                {
                    if (VectorBeginPoint.transform.position.z <= AcceleratorHandle.transform.position.z) //Max point reached can't push it out of bounds
                    {
                        return;
                    }

                    AcceleratorHandle.transform.position -= HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Moving handle forward
                    LastHandPosition = PlayerHand.transform.position;
                    return;
                }


                else
                    Audio.Stop();
                return;
            }


            if (AlmostEqual(HandMovementDirection, HandleMovementDirection, 0.60015f)) //If player is trying to drag the handle towards the direction the handle can move
            {
                if (!Audio.IsPlaying())
                    Audio.Play("lever");

                if (VectorEndPoint.transform.position.z >= AcceleratorHandle.transform.position.z)
                {
                    return;
                }

                else if ((VectorEndPoint.transform.position.z + 0.025f) >= AcceleratorHandle.transform.position.z) //The accelerator has been put to 0, task complete
                {


                    if (trainSpeedHandler.bCanAccelerate)
                        if (!bDisableLever)
                        {
                            bDisableLever = true;
                        }

                    if (trainSpeedHandler.bCanAccelerate)
                        if (BrakeLever.IsTaskCompleted())
                        {
                            trainSpeedHandler.AcceleratorStop();
                            AcceleratorHandle.transform.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Move the handle
                            LastHandPosition = PlayerHand.transform.position;
                            bOnce = false;
                            return;
                        }


                        else
                        {
                            trainSpeedHandler.ChangeSpeed(5);
                            AcceleratorHandle.transform.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Move the handle
                            LastHandPosition = PlayerHand.transform.position;
                            return;
                        }

                }

                AcceleratorHandle.transform.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Move the handle

                if (trainSpeedHandler.bCanAccelerate)
                    if (!bDisableLever)
                    {
                        NewTrainSpeed = Mathf.Lerp(5, 20, normalize01(AcceleratorHandle.transform.position.z, VectorEndPoint.transform.position.z + 0.025f, VectorBeginPoint.transform.position.z));
                        trainSpeedHandler.ChangeSpeed(NewTrainSpeed);
                    }

                LastHandPosition = PlayerHand.transform.position;
                return;
            }

            else if (AlmostEqual(HandMovementDirection, -HandleMovementDirection, 0.60015f)) //If player trying to move handle forward in the direction of the handle
            {
                if (!Audio.IsPlaying())
                    Audio.Play("lever");

                if ((VectorEndPoint.transform.position.z + 0.025f) < AcceleratorHandle.transform.position.z && !BrakeLever.IsTaskCompleted()) //The accelerator has been activated again
                {
                    bDisableLever = false;
                }


                if (VectorBeginPoint.transform.position.z <= AcceleratorHandle.transform.position.z) //Max point reached can't push it out of bounds
                {
                    return;
                }


                AcceleratorHandle.transform.position -= HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Moving handle forward

                NewTrainSpeed = Mathf.Lerp(5, 20, normalize01(AcceleratorHandle.transform.position.z, VectorEndPoint.transform.position.z + 0.025f, VectorBeginPoint.transform.position.z));
                if (trainSpeedHandler.bCanAccelerate)
                    trainSpeedHandler.ChangeSpeed(NewTrainSpeed);
                LastHandPosition = PlayerHand.transform.position;
                return;

            }

            else
                Audio.Stop();
        }
    }

    public override bool hideControllerOnGrab { get { return false; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        if (DriverCabinDoorLock.bIsUnlocked)
        {
            bCanGrab = true;
            PlayerHand = currentController.gameObject;
        }
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();

        bCanGrab = false;
        Audio.Stop();
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (bCanGrab)
        {
            bIsGrabbing = true;
            LastHandPosition = PlayerHand.transform.position;
        }
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        bIsGrabbing = false;
        Audio.Stop();
    }

    private bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }
}

