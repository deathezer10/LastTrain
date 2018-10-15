using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : StationaryObject {

    private bool bSwitchIsOn = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        
    }

    public override void OnControllerExit()
    {
       
    }

    public override void OnControllerStay()
    {
       
    }

    public override void OnGrab()
    {
        
    }

    public override void OnGrabReleased(bool snapped)
    {
      
    }

    public override void OnUse()
    {
        
    }
}
