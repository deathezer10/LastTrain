using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandle : GrabbableObject
{

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
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

    public override void OnGrabReleased()
    {
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

}
