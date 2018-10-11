using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : StationaryObject
{
    public static bool bIsGrabbing = false;
    private GameObject PlayerHand;
    private Transform HandOffsetStart;
    private Transform LastHandLocation;

    private float MaxHandReach = 25.0f;             //Adjust reach before player lets go of the lever
    private float minZRotation = 0.35f;              //Setting lowest reachable rotation for the lever
    private float maxZRotation = 0.0f;               //Setting the max reachable rotation for the lever
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
            if (MaxHandReach < Vector3.Distance(LeverTip.transform.position, PlayerHand.transform.position))
            {
                bIsGrabbing = false;
                return;
            }

            else
            {

               
                Vector3 targetDir = HandOffsetStart.position - parent.transform.position;
                Vector3 targetDirAfter = PlayerHand.transform.position - parent.transform.position;
                float angle = Vector3.Angle(targetDir, targetDirAfter);
                Vector3 cross = Vector3.Cross(targetDir, targetDirAfter);
                if (cross.z < 0) angle = -angle;
                
                if(angle < 0)
                if (currentZRotation <= minZRotation)
                    return;

                if(angle > 0)
                if (currentZRotation >= maxZRotation)
                    return;

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
        print("Controller entered");
        for (int i = 0; i < foundControllers.Length; i++)
        {
            if(foundControllers[i].GetCurrentHand() == handSource)
            {
                PlayerHand = foundControllers[i].gameObject;
                HandOffsetStart = PlayerHand.transform;
                bCanGrab = true;
            }
        }
      
    }

    public override void OnControllerExit()
    {
        print("Controller exited");
        bCanGrab = false;
    }

    public override void OnControllerStay()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUse()
    {
        print("Controller clicked");
        if (bCanGrab)
            bIsGrabbing = true;
    }

}
