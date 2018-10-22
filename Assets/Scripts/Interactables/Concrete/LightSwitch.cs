﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;



public class LightSwitch : StationaryObject
{
    private Light[] lights;
    private bool bSwitchIsOn = false;
    private AudioPlayer Audio;
    
    // Use this for initialization
    void Start()
    {
        lights = FindObjectsOfType(typeof(Light)) as Light[];
        Audio = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        Audio.Play();

        if (handSource.ToString() == SteamVR_Input_Sources.LeftHand.ToString())
            SteamVR_Input.actionsVibration[0].Execute(0, 0.2f, 5, 1, SteamVR_Input_Sources.LeftHand);

        else
            SteamVR_Input.actionsVibration[0].Execute(0, 0.2f, 5, 1, SteamVR_Input_Sources.RightHand);

        
         

        if (bSwitchIsOn)
        {
            bSwitchIsOn = false;
            transform.root.Rotate(new Vector3(0, 0, 1), 80);

            foreach (Light light in lights)
            {
                light.intensity = 0;
            }
        }
       
        else
        {
            bSwitchIsOn = true;
            FindObjectOfType<TrainDoorHandler>().ToggleDoors(false);
            transform.root.Rotate(new Vector3(0,0,-1), 80);
            foreach (Light light in lights)
            {
                light.intensity = 50;
            }
        }
       
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
}
