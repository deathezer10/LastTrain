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

    private void OnTriggerEnter(Collider other)
    {
        if (isDone) return;
        if (DriverCabinDoorLock.bIsUnlocked) return;

        if (successAudio.IsPlaying()) return;
        if (FailedAudio.IsPlaying()) return;

        KeyCard card = other.GetComponent<KeyCard>();

        if (card)
        {
            if (card.IsSuccess()) ScanSuccess(card);
            else ScanFailed(card);
        }
    }

    public void SetGreen()
    {
        transform.parent.GetComponent<Renderer>().materials[0].SetColor("_Color", Color.green);
        transform.parent.GetComponent<Renderer>().materials[0].SetVector("_EmissionColor", Color.green * 100f);
    }

    private void ScanSuccess(KeyCard card)
    {
        if (isDone)
            return;

        if (successAudio) successAudio.Play();

        if (card != null && card.playerController != null)
            card.playerController.Vibration(0, 0.7f, 10, 1, card.playerHand);

        transform.parent.GetComponent<Renderer>().materials[0].SetColor("_Color", Color.green);
        transform.parent.GetComponent<Renderer>().materials[0].SetVector("_EmissionColor", Color.green * 100f);
      
        DriverCabinDoorLock.init();

        isDone = true;
    }

    private void ScanFailed(KeyCard card)
    {
        if (FailedAudio) FailedAudio.Play();
        if(card.playerController != null)
        card.playerController.Vibration(0, 0.3f, 1, 1, card.playerHand);
    }

    public override bool hideControllerOnGrab { get { return false; } }

    public void OnShot(Revolver revolver)
    {
        ScanSuccess(null);
    }
}
