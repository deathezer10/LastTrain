using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class GlassBox : MonoBehaviour
{
    public float thrownBreakForce, heldBreakForce;
    public GameObject brokenGlassPrefab, initialBox, bombObject;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Extinguisher")
        {
            var controller = PlayerViveController.GetControllerThatHolds(other.gameObject);

            if (controller == null)             // If the object has been thrown
            {
                Debug.Log("Thrown Hit with a force of " + other.gameObject.GetComponent<Rigidbody>().velocity.magnitude);

                if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= thrownBreakForce)
                {
                    bombObject.GetComponent<Bomb>().enabled = true;
                    BreakGlass();
                }
            }
            else                                // If the object is held in hand
            {
                Debug.Log("Held Hit with a force of " + controller.gameObject.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity().magnitude);

                if (controller.gameObject.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity().magnitude >= heldBreakForce)
                {
                    bombObject.GetComponent<Bomb>().enabled = true;
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
