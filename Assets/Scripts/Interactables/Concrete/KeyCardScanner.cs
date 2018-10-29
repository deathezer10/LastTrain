using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class KeyCardScanner : StationaryObject
{
    [SerializeField]
    private AudioPlayer successAudio;

    [SerializeField]
    private AudioPlayer FailedAudio;

    private bool isDone = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isDone) return;

        if (successAudio.IsPlaying()) return;
        if (FailedAudio.IsPlaying()) return;

        KeyCard card = other.GetComponent<KeyCard>();

        if (card) {
            if(card.IsSuccess()) ScanSuccess(card);
            else ScanFailed(card);
        }
    }

    private void ScanSuccess(KeyCard card)
    {
        if (successAudio) successAudio.Play();

        card.playerController.Vibration(0, 0.7f, 10, 1, card.playerHand.ToInputSource());

        //Some green led indication perhaps?
        DriverCabinDoorLock.init();

        isDone = true;
    }

    private void ScanFailed(KeyCard card)
    {
        if (FailedAudio) FailedAudio.Play();

        card.playerController.Vibration(0, 0.3f, 1, 1, card.playerHand.ToInputSource());
    }


    private void OnTriggerLeave(Collider other)
    {

    }

    public override void OnControllerEnter(PlayerViveController currentController)
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
    
}
