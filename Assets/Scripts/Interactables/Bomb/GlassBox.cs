using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBox : MonoBehaviour
{
    public float breakReqForce;
    public GameObject brokenGlassPrefab;
    public GameObject initialBox;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>())
        {
            Debug.Log("Hit by " + other.tag + " with a force of " + other.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
        }

        if (other.tag == "Extinguisher" && other.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= breakReqForce)
        {
            Instantiate(brokenGlassPrefab, initialBox.transform.position, initialBox.transform.rotation);
            Destroy(initialBox);
            // TODO Give the pieces of brokenGlassPrefab a portion of the extinguishers rigidbodys velocity.magnitude as force
            // Piece Rigidbodies in an array and foreach .addforce, or just relay on the existing colliders and unity physics

            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

}
