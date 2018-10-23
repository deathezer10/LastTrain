using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class KeyCardScanner : StationaryObject
{
    private float timer = 0.0f;
    private const float TimeToAnalyze = 1.0f;
    private bool bIsCheckingKey = false;
    private const string CardPrefix = "KeyCard_";
    private string UsedCard;
    private bool bIsUnlocked = false;
    private PlayerViveController playerController;
    private HandSource playerHand;
    private AudioPlayer[] Audio;


    // Use this for initialization
    void Start()
    {
        Audio = GetComponents<AudioPlayer>();
    }



    // Update is called once per frame
    void Update()
    {
        if (!bIsUnlocked)
        {
            if (bIsCheckingKey)
            {
                timer += Time.deltaTime;
                int seconds = (int)timer % 60;

                if (timer >= TimeToAnalyze)
                {
                    if (UsedCard.Contains("Right"))
                    {
                        bIsUnlocked = true;
                        foreach (AudioPlayer audio in Audio)
                        {
                            if (audio.clip.name == "keycard_access_1")
                                audio.Play();
                        }

                        playerController.Vibration(0, 0.3f, 1, 1, playerHand.ToInputSource());

                        //Some green led indication perhaps?
                        DriverCabinDoorLock.init();
                    }

                    else
                    {
                        timer = 0.0f;
                        bIsCheckingKey = false;
                        foreach (AudioPlayer audio in Audio)
                        {
                            if (audio.clip.name == "access_denied")
                                audio.Play();
                        }
                        
                        playerController.Vibration(0, 0.7f, 10, 1, playerHand.ToInputSource());

                        //Wrong keycard tried, some red led indications also perhaps ?
                    }
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains(CardPrefix))
        {
            bIsCheckingKey = true;
            UsedCard = other.gameObject.name;
        }
    }


    private void OnTriggerLeave(Collider other)
    {
        if (other.gameObject.name.Contains(CardPrefix))
        {
            bIsCheckingKey = false;
            timer = 0.0f;
            UsedCard = null;
        }
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        playerController = currentController;
        playerHand = playerController.GetCurrentHand();
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
