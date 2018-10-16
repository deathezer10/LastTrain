using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class KeyCardScanner : MonoBehaviour
{
    private float timer = 0.0f;
    private float TimeToAnalyze = 2.0f;
    private bool bIsCheckingKey = false;
    private string CardPrefix = "KeyCard_";
    private string UsedCard;

    // Use this for initialization
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {

        if (bIsCheckingKey)
        {
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
        }

        
        if(timer >= TimeToAnalyze)
        {
            if(UsedCard.Contains("Right"))
            {
                //Some green led indication perhaps?
                DriverCabinDoorLock.init();
            }

            else
            {
                timer = 0.0f;
                bIsCheckingKey = false;
                SteamVR_Input.actionsVibration[0].Execute(0, 0.3f, 5, 1, SteamVR_Input_Sources.LeftHand);
                SteamVR_Input.actionsVibration[0].Execute(0, 0.3f, 5, 1, SteamVR_Input_Sources.RightHand);
                //Wrong keycard tried, some red led indications also perhaps ?
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
}
