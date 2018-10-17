﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suitcase : GrabbableObject
{
    public GameObject[] keycardArray;

    Animator suitcaseAnimator;
    bool opened;

    void Start()
    {
        suitcaseAnimator = GetComponent<Animator>();
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            OpenSuitCase();
        }
    }
    */

    public void OpenSuitCase()
    {
        suitcaseAnimator.Play("suitcase_open");

        foreach (GameObject go in keycardArray)
        {
            go.SetActive(true);
            go.transform.SetParent(null);
        }

        BoxCollider[] suitCaseColliders = GetComponents<BoxCollider>();

        foreach (BoxCollider bc in suitCaseColliders)
        {
            bc.enabled = true;
        }
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
        if (!opened)
        {
            OpenSuitCase();
            opened = true;
        }
    }
}
