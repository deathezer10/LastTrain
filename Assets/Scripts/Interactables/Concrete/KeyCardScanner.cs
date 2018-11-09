using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class KeyCardScanner : StationaryObject, IShootable
{
    [SerializeField]
    private AudioPlayer successAudio;

    [SerializeField]
    private AudioPlayer FailedAudio;

    private bool isDone = false;

    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isDone) return;

        if (successAudio.IsPlaying()) return;
        if (FailedAudio.IsPlaying()) return;

        KeyCard card = other.GetComponent<KeyCard>();

        if (card)
        {
            if (card.IsSuccess()) ScanSuccess(card);
            else ScanFailed(card);
        }
    }

    private void ScanSuccess(KeyCard card)
    {
        if (isDone)
            return;

        if (successAudio) successAudio.Play();

        if (card != null && card.playerController != null)
            card.playerController.Vibration(0, 0.7f, 10, 1, card.playerHand);

        transform.parent.GetComponent<Renderer>().materials[1].SetColor("_Color", Color.green);
        transform.parent.GetComponent<Renderer>().materials[1].SetVector("_EmissionColor", Color.green * 100f);
      
        DriverCabinDoorLock.init();

        isDone = true;
    }

    private void ScanFailed(KeyCard card)
    {
        if (FailedAudio) FailedAudio.Play();
        if(card.playerController != null)
        card.playerController.Vibration(0, 0.3f, 1, 1, card.playerHand);
    }


    private void OnTriggerLeave(Collider other)
    {

    }

    public override bool hideControllerOnGrab { get { return false; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);
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
        base.OnGrab();
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
        ScanSuccess(null);
    }
}
