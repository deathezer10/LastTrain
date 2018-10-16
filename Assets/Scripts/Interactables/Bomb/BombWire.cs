using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWire : MonoBehaviour
{
    public bool correctWire;
    public Color wrongCutColor, rightCutColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GlassFragment")
        {
            var controller = PlayerViveController.GetControllerThatHolds(other.gameObject);

            // Check if player is holding the glass and a piece hasnt just randomly fallen on the wire collider
            if (controller != null)
            {
                Debug.Log("Trigger Wire cut state / animation / model change.");

                if (correctWire)
                {
                    Debug.Log("Bomb diffused, trigger next sequence.");
                    GetComponent<MeshRenderer>().material.color = rightCutColor;
                    // Diffuse bomb - Display change in bomb state - Trigger next sequence
                }
                else
                {
                    Debug.Log("Bomb exploded, trigger death state.");
                    GetComponent<MeshRenderer>().material.color = wrongCutColor;
                    // Explode - Death notification
                }
            }
            
        }
    }
}
