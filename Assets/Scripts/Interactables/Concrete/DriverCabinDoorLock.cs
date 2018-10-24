using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DriverCabinDoorLock : StationaryObject
{

    public static DriverCabinDoorLock instance;
    private GameObject PlayerHand;
    private Rigidbody doorBody;
    private BoxCollider DoorMesh;

    public static bool bIsUnlocked = false;
    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private bool bIsLastleft = false;
    private bool Awaked = false;

    private Vector3 LastHandPosition;
    private Vector3 HandleMovementDirection = new Vector3(1, 0, 0);
    private Vector3 LeftDoorCorner;
    private Vector3 RightDoorCorner;
    private Vector3 DoorSlamVelocity;
    private Vector3 Velocitystart;
    private Vector3 VelocityEnd;

    private float timer;
    private float velocity;
    private float timed;
    private float distance;
    private float TimefromGrab;
    private float ReleasedTime;
  
    void Start()
    {
        doorBody = transform.parent.GetComponent<Rigidbody>();
        DoorMesh = transform.parent.gameObject.GetComponent<BoxCollider>();
        RightDoorCorner = DoorMesh.transform.TransformPoint(DoorMesh.center + new Vector3(DoorMesh.size.x, -DoorMesh.size.y, DoorMesh.size.z) * 0.5f);
        LeftDoorCorner = DoorMesh.transform.TransformPoint(DoorMesh.center + new Vector3(-DoorMesh.size.x, -DoorMesh.size.y, DoorMesh.size.z) * 0.5f);
    }

    
    void Update()
    {
        if (!bDisableLever)
        {
            if(Awaked)
            {
                timer = Time.time;

                if (DoorMesh.transform.TransformPoint(DoorMesh.center + new Vector3(DoorMesh.size.x, -DoorMesh.size.y, DoorMesh.size.z) * 0.32f).x <= LeftDoorCorner.x)
                {
                    if (doorBody.velocity.x < -0.01f)
                    {
                        DoorSlamVelocity = -doorBody.velocity;
                        doorBody.velocity = Vector3.zero;
                        doorBody.velocity = HandleMovementDirection * (DoorSlamVelocity.x / 4);
                        return;
                    }
                }

                if (transform.TransformPoint(DoorMesh.center + new Vector3(DoorMesh.size.x, -DoorMesh.size.y, DoorMesh.size.z) * 0.5f).x >= RightDoorCorner.x)
                {
                        doorBody.velocity = Vector3.zero;
                }

                if (bIsGrabbing)
                {
                    if (FastApproximately(0, velocity, 0.2f))
                    {
                        print("Velocity almost zero");
                        TimefromGrab = Time.time;
                        Velocitystart = PlayerHand.transform.position;
                    }

                    Vector3 HandMovementDirection = PlayerHand.transform.position - LastHandPosition;
                    HandMovementDirection.Normalize();

                    if (AlmostEqual(HandMovementDirection, HandleMovementDirection, 0.40015f))
                    {
                        if (DoorMesh.transform.TransformPoint(DoorMesh.center + new Vector3(DoorMesh.size.x, -DoorMesh.size.y, DoorMesh.size.z) * 0.5f).x >= RightDoorCorner.x)
                        {
                            print("right limit reached");
                            return;
                        }

                        if (bIsLastleft)
                        {
                            TimefromGrab = Time.time;
                            Velocitystart = PlayerHand.transform.position;
                        }

                        transform.parent.position += HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position);
                        timed = Time.time - timer;
                        distance = Vector3.Distance(PlayerHand.transform.position, LastHandPosition);
                        velocity = distance / timed;
                        LastHandPosition = PlayerHand.transform.position;

                        bIsLastleft = false;
                        return;
                    }

                    if (AlmostEqual(HandMovementDirection, -HandleMovementDirection, 0.40015f))
                    {
                        if (DoorMesh.transform.TransformPoint(DoorMesh.center + new Vector3(DoorMesh.size.x, -DoorMesh.size.y, DoorMesh.size.z) * 0.34f).x <= LeftDoorCorner.x)
                        {
                            print("Left limit reached");
                            return;
                        }

                        if (!bIsLastleft)
                        {
                            TimefromGrab = Time.time;
                            Velocitystart = PlayerHand.transform.position;
                        }

                        transform.parent.position -= HandleMovementDirection * Vector3.Distance(LastHandPosition, PlayerHand.transform.position);
                        timed = Time.time - timer;
                        distance = Vector3.Distance(PlayerHand.transform.position, LastHandPosition);
                        velocity = distance / timed;
                        LastHandPosition = PlayerHand.transform.position;
                        bIsLastleft = true;
                        return;
                    }

                    timed = Time.time - timer;
                    distance = Vector3.Distance(PlayerHand.transform.position, LastHandPosition);
                    velocity = distance / timed;
                }
            }
        }
    }

    void Awake()
    {
        instance = this;
    }

    public static void init()
    {
        bIsUnlocked = true;
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        Awaked = true;
        if (bIsUnlocked)
        {
            bCanGrab = true;
            PlayerHand = currentController.gameObject;
        }
    }

    public override void OnControllerExit()
    {
        if(bIsGrabbing)
        {
            VelocityEnd = PlayerHand.transform.position;
            ReleasedTime = Time.time - TimefromGrab;
            float vel = Vector3.Distance(Velocitystart, VelocityEnd) / ReleasedTime;


            if (bIsLastleft)
                doorBody.velocity = -HandleMovementDirection * vel;


            else
                doorBody.velocity = HandleMovementDirection * vel;

        }

        bCanGrab = false;
        bIsGrabbing = false;
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
            Velocitystart = PlayerHand.transform.position;
            TimefromGrab = Time.time;
            doorBody.velocity = Vector3.zero;
        }
    }

    public override void OnGrabReleased()
    {
        if(bIsGrabbing)
        {
            VelocityEnd = PlayerHand.transform.position;
            ReleasedTime = Time.time - TimefromGrab;
            float vel = Vector3.Distance(Velocitystart, VelocityEnd) / ReleasedTime;

            if (bIsLastleft)
                doorBody.velocity = -HandleMovementDirection * vel;

            else
                doorBody.velocity = HandleMovementDirection * vel;
        }
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


    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
