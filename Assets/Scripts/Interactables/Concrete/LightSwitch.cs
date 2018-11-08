﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;

public class LightSwitch : GrabbableObject , IShootable
{
    List<Light> m_TrainLights = new List<Light>();
    private bool bSwitchIsOn = false;
    private bool bIsBroken = false;
    private int ActivateCount = 0;
    private int BreakAtCount = 3;
    private AudioPlayer Audio;
    

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_TrainLights.Add(transform.GetChild(i).GetComponent<Light>());
        }

        Audio = GetComponent<AudioPlayer>();
        
    }

    public override bool hideControllerOnGrab { get { return false; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        if(bIsBroken)
            return;

        if(ActivateCount >= BreakAtCount)
        {
            //Light break sound here?
            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }
            bIsBroken = true;
            transform.GetComponent<Rigidbody>().useGravity = true;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            return;
        }

        Audio.Play();

        var source = currentController.GetCurrentHand();
        currentController.Vibration(0, 0.2f, 5, 1, source);

        if (bSwitchIsOn)
        {
            bSwitchIsOn = false;
            
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }
        }
        else
        {
            bSwitchIsOn = true;
            ActivateCount += 1;
            transform.localRotation = Quaternion.Euler(0, 0, -90);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(true);
            }
        }

    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
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

    public void OnShot(Revolver revolver)
    {
        if(bSwitchIsOn)
        {
            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }
        }

        Destroy(transform.gameObject);
        Destroy(this);

    }
}
