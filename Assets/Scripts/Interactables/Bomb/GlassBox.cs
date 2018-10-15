using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class GlassBox : MonoBehaviour
{
    public float thrownBreakForce, heldBreakForce;
    public GameObject brokenGlassPrefab;
    public GameObject initialBox;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            Debug.Log("Hit by " + other.tag + " with a force of " + other.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        }

        if (other.tag == "Extinguisher")
        {
            var controller = PlayerViveController.GetControllerThatHolds(other.gameObject);

            if (controller == null)     // If the object has been thrown
            {
                if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= thrownBreakForce)
                {
                    BreakGlass();
                }
            }
            else                        // If the object is held in hand
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
        Instantiate(brokenGlassPrefab, initialBox.transform.position, initialBox.transform.rotation);
        Destroy(initialBox);

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

}
