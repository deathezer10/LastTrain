using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class SawBlade : GrabbableObject
{
    PlayerViveController playerController;
    HandSource playerHand;
    AudioPlayer spinAudio;
    
    bool spinning, held;
    float vibrationTimer;
    Animator sawBladeAnimator;
    
    private void Start()
    {
        sawBladeAnimator = GetComponent<Animator>();
        spinAudio = GetComponent<AudioPlayer>();
    }

    private void Update()
    {
        if (spinning && held)
        {
            if (Time.time >= vibrationTimer)
            {
                var source = playerHand.ToInputSource();
                playerController.Vibration(0, 0.2f, 0.2f, 0.7f, source);
                vibrationTimer = Time.time + 0.2f;
            }
        }
    }

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        playerController = currentController;
        playerHand = playerController.GetCurrentHand();
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
        
        spinning = false;
        held = false;

        sawBladeAnimator.Play("SawBladeStop");
        spinAudio.Stop();
    }

    public override void OnControllerStay()
    {
    }

    public override void OnGrab()
    {
        held = true;

        transform.rotation = playerController.transform.rotation;
        transform.Rotate(new Vector3(0, 180, 0));
        transform.position = playerController.transform.position;
    }

    public override void OnGrabReleased()
    {
        spinning = false;
        held = false;

        sawBladeAnimator.Play("SawBladeStop");
        spinAudio.Stop();
    }

    public override void OnUse()
    {
        if (held)
        {
            spinning = spinning ? false : true;

            if (spinning)
            {
                sawBladeAnimator.Play("SawBladeSpin");
                spinAudio.Play();
                vibrationTimer = Time.time;
            }
            else
            {
                sawBladeAnimator.Play("SawBladeStop");
                spinAudio.Stop();
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
