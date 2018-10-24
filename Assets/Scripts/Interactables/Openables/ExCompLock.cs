using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExCompLock : GrabbableObject
{
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
        GetComponent<BoxCollider>().enabled = false;
        FindObjectOfType<ExtghsrCompartment>().OpenExCompartment();
    }
}
