using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;

public class LightSwitch : GrabbableObject , IShootable
{
  

    public string SwitchCabinsName;
    private bool bSwitchIsOn = false;
    private bool bIsBroken = false;
    private int ActivateCount = 0;
    private int BreakAtCount;
    private AudioPlayer Audio;
    private List<ToggleTrainLights> toggleTrainLights = new List<ToggleTrainLights>();
    private ToggleTrainLights ownedLights;

    private void Start()
    {
        Audio = GetComponent<AudioPlayer>();
        BreakAtCount = Mathf.RoundToInt(Random.Range(3, 8));
        toggleTrainLights.AddRange(FindObjectsOfType<ToggleTrainLights>());
        for(int i = 0; i < toggleTrainLights.Count; i++)
        {
            if(toggleTrainLights[i].SwitchCabinsName == SwitchCabinsName)
            {
                ownedLights = toggleTrainLights[i];
            }
        }
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
            ownedLights.LightsOff();
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

            ownedLights.LightsOff();
        }
        else
        {
            bSwitchIsOn = true;
            ActivateCount += 1;
            transform.localRotation = Quaternion.Euler(0, 0, -90);

            ownedLights.LightsOn();
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
            ownedLights.LightsOff();
        }

        Destroy(transform.gameObject);
        Destroy(this);

    }
}
