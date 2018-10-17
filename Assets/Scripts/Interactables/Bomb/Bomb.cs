using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : GrabbableObject
{
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

    public override void OnGrabReleased()
    {
    }

    public override void OnUse()
    {
    }

    public void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }
}