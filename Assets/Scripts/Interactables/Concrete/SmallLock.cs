﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLock : GrabbableObject
{
    public GameObject uncutTopPart, cutTopPart, umbrella;

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
        if (other.tag == "SawBlade" && other.GetComponentInParent<SawBlade>().IsSpinning())
        {
            Debug.Log("Small lock hit with sawblade.");

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
}