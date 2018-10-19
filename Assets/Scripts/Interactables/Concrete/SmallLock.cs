using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLock : GrabbableObject
{
    public GameObject uncutTopPart, cutTopPart;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SawBlade" && other.gameObject.GetComponent<SawBlade>().IsSpinning())
        {
            BreakPieces();
            UnlockRigidbody();
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    private void BreakPieces()
    {
        Debug.Log("Break pieces apart");
        uncutTopPart.SetActive(false);
        cutTopPart.SetActive(true);
        cutTopPart.transform.SetParent(null);
    }

    private void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }
}