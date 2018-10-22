using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;



public class LightSwitch : StationaryObject
{
    List<Light> m_TrainLights = new List<Light>();
    private bool bSwitchIsOn = false;

    private AudioPlayer Audio;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_TrainLights.Add(transform.GetChild(i).GetComponent<Light>());
            Audio = GetComponent<AudioPlayer>();
        }
    }

    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        Audio.Play();

        SteamVR_Input.actionsVibration[0].Execute(0, 0.2f, 5, 1, currentController.HandSourceToInputSource());

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

            FindObjectOfType<TrainDoorHandler>().ToggleDoors(false, () => {
                FindObjectOfType<StationMover>().ToggleMovement(true);
                FindObjectOfType<TrainTimeHandler>().StartTrainTime(); });
            
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
