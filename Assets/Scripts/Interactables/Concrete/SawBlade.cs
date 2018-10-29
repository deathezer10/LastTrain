using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class SawBlade : GrabbableObject
{
    public Color PH_SpinColor, PH_NotSpinColor;
    public GameObject PH_SpinIndicator;  // To be replaced with sound / animation for spin

    private PlayerViveController playerController;
    private HandSource playerHand;
    
    bool spinning, held;
    float vibrationTimer;
    Animator sawBladeAnimator;
    
    private void Start()
    {
        sawBladeAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (spinning && held)
        {
            if (playerController != null && Time.time >= vibrationTimer)
            {
                var source = playerHand.ToInputSource();
                playerController.Vibration(0, 0.3f, 0.3f, 0.5f, source);
                vibrationTimer = Time.time + 0.3f;
            }
        }
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        playerController = currentController;
        playerHand = playerController.GetCurrentHand();
    }

    public override void OnControllerExit()
    {
        playerController = null;
        held = false;
    }

    public override void OnControllerStay()
    {
    }

    public override void OnGrab()
    {
        held = true;
    }

    public override void OnGrabReleased()
    {
        spinning = false;
        held = false;

        PH_SpinIndicator.GetComponent<MeshRenderer>().material.color = PH_NotSpinColor;

        sawBladeAnimator.Play("SawBladeStop");

        playerController = null;
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
                vibrationTimer = Time.time + 0.3f;
            }
            else
            {
                sawBladeAnimator.Play("SawBladeStop");
            }
        }
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

    public bool IsSpinning()
    {
        return spinning;
    }
}
