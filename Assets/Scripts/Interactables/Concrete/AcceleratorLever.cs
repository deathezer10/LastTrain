using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorLever : StationaryObject
{
    public static AcceleratorLever instance;
    private GameObject Accelerator;
    private GameObject AcceleratorHandle;
    private BoxCollider HandleCollider;
    private GameObject PlayerHand;
    private GameObject VectorBeginPoint;
    private GameObject VectorEndPoint;

    private Vector3 HandleMovementDirection;
    private Vector3 LastHandPosition;
    private Vector3 HandleDefaultMaxPosition;

    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private AudioPlayer Audio;

    //Static function for brakelever to check if this AcceleratorLever is engaged.
    public static bool IsTaskCompleted()
    {
        return instance.bDisableLever;
    }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        HandleCollider = GetComponent<BoxCollider>();
        Accelerator = transform.parent.gameObject;
        AcceleratorHandle = HandleCollider.gameObject;
        HandleDefaultMaxPosition = AcceleratorHandle.transform.position;
        VectorBeginPoint = Accelerator.transform.GetChild(2).gameObject;
        VectorEndPoint = Accelerator.transform.GetChild(1).gameObject;
        HandleMovementDirection = VectorEndPoint.transform.position - VectorBeginPoint.transform.position;
        HandleMovementDirection.Normalize(); //The direction where Acceleratorhandle can be moved forth and back.
        Audio = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bDisableLever) //If player completed this task no more updating extra stuff.
        {
            if (bIsGrabbing) //Player is grabbing the acceleratorhandle
            {
                Vector3 HandMovementDirection = PlayerHand.transform.position - LastHandPosition; //We get the small movement vector of player's hand
                HandMovementDirection.Normalize();
                if (AlmostEqual(HandMovementDirection, HandleMovementDirection, 0.40015f)) //If player is trying to drag the handle towards the direction the handle can move
                {
                    if (VectorEndPoint.transform.position.z >= AcceleratorHandle.transform.position.z) //The accelerator has been put to 0, task complete
                    {
                        bIsGrabbing = false;
                        bDisableLever = true;
                        Audio.Play();

                        if (BrakeLever.IsTaskCompleted())
                        {
                            //This was last lever,stop train&do something
                            return;
                        }

                        else
                            return; //Player still needs to engage the brakelever..
                    }

                    AcceleratorHandle.transform.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Move the handle
                    LastHandPosition = PlayerHand.transform.position; 
                    return;
                }

                if (AlmostEqual(HandMovementDirection, -HandleMovementDirection, 0.40015f)) //If player trying to move handle forward in the direction of the handle
                {
                    if (HandleDefaultMaxPosition.z <= AcceleratorHandle.transform.position.z) //Max point reached can't push it out of bounds
                    {
                        return;
                    }

                    AcceleratorHandle.transform.position -= HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position); //Moving handle forward
                    LastHandPosition = PlayerHand.transform.position;
                    return;

                }








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

    }

    public override void OnGrab()
    {
        if (bCanGrab)
        {
            bIsGrabbing = true;
            LastHandPosition = PlayerHand.transform.position;
        }
    }

    public override void OnGrabReleased()
    {
        bIsGrabbing = false;
    }

    public override void OnUse()
    {

    }


    private bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

}
