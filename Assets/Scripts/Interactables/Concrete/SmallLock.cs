using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLock : GrabbableObject, IShootable
{
    public GameObject uncutTopPart, cutTopPart, umbrella;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SawBlade" && other.GetComponentInParent<SawBlade>().IsSpinning())
        {
            BreakLock();
        }
    }

    private void BreakLock()
    {
        BreakPieces();
        UnlockRigidbody();
        GetComponent<CapsuleCollider>().enabled = false;
        FindObjectOfType<UmbrCompartment>().OpenCompartment();
        umbrella.SetActive(true);
        umbrella.transform.SetParent(null);
    }

    private void BreakPieces()
    {
        uncutTopPart.SetActive(false);
        cutTopPart.SetActive(true);
        cutTopPart.transform.SetParent(null);
    }

    private void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }

    public void OnShot(Revolver revolver)
    {
        BreakLock();
    }
}