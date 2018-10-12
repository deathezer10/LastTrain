using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : StationaryObject
{
    
    public static bool bIsGrabbing = false;
    private GameObject PlayerHand;
    private Transform HandOffsetStart;
    private Transform LastHandLocation;
    private string WhichHand;
    private int HandIndex = -1;

    private float MaxHandReach = 10.0f;              //Adjust reach before player lets go of the lever
    private float minZRotation = -0.80f;              //Setting lowest reachable rotation for the lever
    private float maxZRotation = 0.80f;               //Setting the max reachable rotation for the lever
    private float currentZRotation = 0.0f;

    private bool bCanGrab = false;
    private PlayerViveController[] foundControllers;
    private GameObject parent;
    private BoxCollider LeverTip;

    // Use this for initialization
    void Start()
    {
        foundControllers = FindObjectsOfType<PlayerViveController>();
        parent = transform.root.gameObject;
        LeverTip = GetComponent<BoxCollider>();
        currentZRotation = parent.transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {

        if (bIsGrabbing)
        {
            PlayerHand.transform.position = foundControllers[HandIndex].gameObject.transform.position;
            if (MaxHandReach < Vector3.Distance(LeverTip.transform.position, PlayerHand.transform.position))
            {
                Debug.Log("hello world");
                bIsGrabbing = false;
                bCanGrab = false;
                return;
            }

            else
            {
                print("Else reached");
               
                Vector3 targetDir = HandOffsetStart.position - parent.transform.position;
                Vector3 NewtargetDir = PlayerHand.transform.position - parent.transform.position;
                float angle = Vector3.Angle(targetDir, NewtargetDir);

                //if (targetDir.z > targetDirAfter.z)
                  //  angle = -angle;
         
            
                Vector3 cross = Vector3.Cross(targetDir, NewtargetDir);
                if (cross.z < 0) angle = -angle;



                if (angle < 0)
                    if (currentZRotation <= minZRotation)
                    {
                        Debug.Log("Minimum lever angle reached");
                        bIsGrabbing = false;                     
                        return;
                    }

                if (angle > 0)
                    if (currentZRotation >= maxZRotation)
                    {
                        Debug.Log("Maximum lever angle reached");
                        bIsGrabbing = false;
                        return;
                    }
                parent.transform.Rotate(0, 0, angle);
                HandOffsetStart = PlayerHand.transform;

            }

            
        }



    }

    public static bool IsGrabbing()
    {
        return bIsGrabbing;
    }
        
    public override void OnControllerEnter(PlayerViveController.HandSource handSource)
    {
        bCanGrab = true;
        WhichHand = handSource.ToString();
    }

    public override void OnControllerExit()
    {
        bCanGrab = false;
    }

    public override void OnControllerStay()
    {

    }

    public override void OnUse()
    {
       
    }

    public override void OnGrab()
    {
        print("Grabbed");
        if (bCanGrab)
        {
            for (int i = 0; i < foundControllers.Length; i++)
            {
                if (foundControllers[i].GetCurrentHand().ToString() == WhichHand)
                {
                    print("Should print once");
                    PlayerHand = foundControllers[i].gameObject;
                    HandIndex = i;
                    HandOffsetStart = PlayerHand.transform;
                }
            }
            bIsGrabbing = true;
        }
           
    }

    public override void OnGrabReleased(bool snapped)
    {
        print("Let Go");
        bIsGrabbing = false;
    }
    
}
