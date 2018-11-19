using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollRange : MonoBehaviour
{
    Doll dollScript;

    private void Start()
    {
        dollScript = GetComponentInParent<Doll>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dollScript.PlayerWithinRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            dollScript.PlayerWithinRange(false);
        }
    }
}
