using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;



public class LightSwitch : StationaryObject
{
    List<Light> m_TrainLights = new List<Light>();
    private bool bSwitchIsOn = false;
    Audio = GetComponent<AudioPlayer>();
    private AudioPlayer Audio;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_TrainLights.Add(transform.GetChild(i).GetComponent<Light>());
        }
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

            transform.localRotation = Quaternion.Euler(0, 0, 0);

            foreach (Light light in m_TrainLights)
            {
                light.enabled = false;
            }
        }
        else
        {
            bSwitchIsOn = true;

            FindObjectOfType<TrainDoorHandler>().ToggleDoors(false, () => { FindObjectOfType<StationMover>().ToggleMovement(true); });

            transform.localRotation = Quaternion.Euler(0, 0, -90);

            foreach (Light light in m_TrainLights)
            {
                light.enabled = true;
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
