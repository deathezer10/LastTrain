using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class SawBlade : GrabbableObject
{
    public Color PH_SpinColor, PH_NotSpinColor;
    public GameObject PH_SpinIndicator;  // To be replaced with sound / animation for spin

    string holdingControllerHand;
    bool spinning, held;
    Animator sawBladeAnimator;

    private void Start()
    {
        sawBladeAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (spinning && held)
        {
            if (holdingControllerHand == SteamVR_Input_Sources.LeftHand.ToString())
            {
                SteamVR_Input.actionsVibration[0].Execute(0, 0.3f, 0.3f, 0.8f, SteamVR_Input_Sources.LeftHand);
            }
            else
            {
                SteamVR_Input.actionsVibration[0].Execute(0, 0.3f, 0.3f, 0.8f, SteamVR_Input_Sources.RightHand);
            }
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
        held = true;

        holdingControllerHand = PlayerViveController.GetControllerThatHolds(gameObject).GetCurrentHand().ToString();
    }

    public override void OnGrabReleased()
    {
        spinning = false;
        held = false;

        PH_SpinIndicator.GetComponent<MeshRenderer>().material.color = PH_NotSpinColor;

        sawBladeAnimator.Play("SawBladeStop");

        holdingControllerHand = "";
    }

    public override void OnUse()
    {
        if (held)
        {
            spinning = spinning ? false : true;

            PH_SpinIndicator.GetComponent<MeshRenderer>().material.color = spinning ? PH_SpinColor : PH_NotSpinColor;

            if (spinning)
            {
                sawBladeAnimator.Play("SawBladeSpin");
            }
            else
            {
                sawBladeAnimator.Play("SawBladeStop");
            }
        }
    }

    public bool IsSpinning()
    {
        return spinning;
    }
}
