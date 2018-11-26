﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollRange : MonoBehaviour
{
    Doll dollScript;

    private void Start()
    {
        dollScript = GetComponentInParent<Doll>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerHead")
        {
            dollScript.PlayerWithinRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerHead")
        {
            dollScript.PlayerWithinRange(false);
        }
    }
}
