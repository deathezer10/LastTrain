using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class BombWire : MonoBehaviour
{
    public float heldCutForce;
    public bool correctWire;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GlassFragment")
        {
            var controller = PlayerViveController.GetControllerThatHolds(other.gameObject);

            // Check if player is holding the glass and a piece hasn't just randomly landed on the wire collider
            if (controller != null && controller.gameObject.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity().magnitude >= heldCutForce)
            {
                Debug.Log("Trigger Wire cut state / animation / model change.");

                Bomb bomb = FindObjectOfType<Bomb>();

                if (correctWire)
                {
                    Debug.Log("Bomb diffused, trigger next sequence.");
                    bomb.CutRightWire();
                    // Diffuse bomb - Display change in bomb state - Trigger next sequence
                }
                else
                {
                    Debug.Log("Bomb exploded, trigger death state.");
                    bomb.CutWrongWire();
                    // Explode - Death state
                }

                SwapWireModel();
            }
            
        }
    }

    private void SwapWireModel()
    {
        MeshRenderer[] wireRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mR in wireRenderers)
        {
            if (mR.enabled)
            {
                mR.enabled = false;
            }
            else
            {
                mR.enabled = true;
            }
        }
    }
}
