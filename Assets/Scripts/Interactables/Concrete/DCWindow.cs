using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class DCWindow : MonoBehaviour
{
    public float thrownBreakForce, heldBreakForce;
    public GameObject initialWindow;
    public GameObject[] brokenWindowPieces;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Extinguisher")
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
}
