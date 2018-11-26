using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class DCWindow : MonoBehaviour, IShootable
{
    public GameObject initialWindow;
    public GameObject[] brokenWindowPieces;

    private float thrownBreakForce = 1.8f, heldBreakForce = 2.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Extinguisher" ||other.tag == "BaseballBat" || other.tag == "BowlingBall")
        {
            var controller = PlayerViveController.GetControllerThatHolds(other.gameObject);

            if (controller == null)
            {
                if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= thrownBreakForce)
                {
                    BreakGlass();
                }
            }
            else
            {
                if (controller.gameObject.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity().magnitude >= heldBreakForce)
                {
                    BreakGlass();
                }
            }
        }
    }

    private void BreakGlass()
    {
        Destroy(initialWindow);

        foreach (GameObject go in brokenWindowPieces)
        {
            go.SetActive(true);
        }

        GetComponent<AudioPlayer>().Play();

        GetComponent<BoxCollider>().enabled = false;
    }
    
    public void OnShot(Revolver revolver)
    {
        BreakGlass();
    }
}
