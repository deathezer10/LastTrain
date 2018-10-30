using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class GlassBox : MonoBehaviour, IShootable
{
    public float thrownBreakForce, heldBreakForce;
    public GameObject brokenGlassPrefab, initialBox;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Extinguisher")
        {
            var controller = PlayerViveController.GetControllerThatHolds(other.gameObject);

            if (controller == null)             // If the object has been thrown
            {
                if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= thrownBreakForce)
                {
                    BreakGlass();
                }
            }
            else                                // If the object is held in hand
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
        FindObjectOfType<Bomb>().UnlockRigidbody();

        Instantiate(brokenGlassPrefab, initialBox.transform.position, initialBox.transform.rotation);
        Destroy(initialBox);
        GetComponent<AudioPlayer>().Play();

        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void OnShot(Revolver revolver)
    {
        BreakGlass();
    }
}
