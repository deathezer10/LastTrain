using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class KeyCardScanner : StationaryObject
{
    private float timer = 0.0f;
    private float TimeToAnalyze = 0.7f;
    private bool bIsCheckingKey = false;
    private string CardPrefix = "KeyCard_";
    private string UsedCard;
    private bool bIsUnlocked = false;
    private string VibrationHand;

    // Use this for initialization
    void Start()
    {

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
                        print("Right card used");
                        bIsUnlocked = true;

                        if (VibrationHand == SteamVR_Input_Sources.LeftHand.ToString())
                            SteamVR_Input.actionsVibration[0].Execute(0, 0.3f, 1, 1, SteamVR_Input_Sources.LeftHand);
                        else
                            SteamVR_Input.actionsVibration[0].Execute(0, 0.3f, 1, 1, SteamVR_Input_Sources.RightHand);
                        //Some green led indication perhaps?
                        DriverCabinDoorLock.init();
                    }

                    else
                    {
                        timer = 0.0f;
                        bIsCheckingKey = false;
                        if (VibrationHand == SteamVR_Input_Sources.LeftHand.ToString())
                            SteamVR_Input.actionsVibration[0].Execute(0, 1.0f, 10, 1, SteamVR_Input_Sources.LeftHand);
                        else
                            SteamVR_Input.actionsVibration[0].Execute(0, 1.0f, 10, 1, SteamVR_Input_Sources.RightHand);
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

    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        VibrationHand = handSource.ToString();
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

    public override void OnGrabReleased(bool snapped)
    {

    }

    public override void OnUse()
    {

    }
}
