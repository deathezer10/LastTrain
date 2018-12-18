using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class SawBlade : GrabbableObject
{
    PlayerViveController playerController;
    SteamVR_Input_Sources playerHand;
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
                var source = playerHand;
                playerController.Vibration(0, 0.2f, 0.2f, 0.7f, source);
                vibrationTimer = Time.time + 0.2f;
            } else
            {
                playerController.Vibration(0, 0, 0, 0, playerHand);
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

    public override void OnGrab()
    {
        base.OnGrab();

        held = true;

        if (playerController == null) return;
        transform.rotation = playerController.transform.rotation;
        transform.Rotate(new Vector3(0, 180, 0));
        transform.position = playerController.transform.position;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

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

    public bool IsSpinning()
    {
        return spinning;
    }
}
