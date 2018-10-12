using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorLever : StationaryObject
{

    BoxCollider HandleCollider;
    private bool bCanGrab = false;
    private bool bIsGrabbing = false;
    private bool bDisableLever = false;
    private GameObject AcceleratorHandle;
    private float HandleMaxPosition;
    private float HandleMinPosition = 0.5f;
    private GameObject PlayerHand;
    // Use this for initialization
    void Start()
    {
        HandleCollider = GetComponent<BoxCollider>();
        AcceleratorHandle = HandleCollider.gameObject;
        HandleMaxPosition = AcceleratorHandle.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bDisableLever)
        {
            if (bIsGrabbing)
            {

                if (PlayerHand.transform.position.z > HandleMaxPosition)
                {
                    if (AcceleratorHandle.transform.position.z >= HandleMinPosition)
                    {
                        //TODO: FUNCTION(s) for pulling Acceleration to zero
                        bDisableLever = true;
                        bIsGrabbing = false;
                    }
                    
                    else
                        AcceleratorHandle.transform.SetPositionAndRotation(new Vector3(AcceleratorHandle.transform.position.x,
                            AcceleratorHandle.transform.position.y, PlayerHand.transform.position.z), AcceleratorHandle.transform.rotation);

                    //AcceleratorHandle.transform.Translate(0, 0, PlayerHand.transform.position.z);
                    
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
        }
    }

    public override void OnGrabReleased(bool snapped)
    {
        bIsGrabbing = false;
    }

    public override void OnUse()
    {

    }




}
