﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleGantryBarrier : MonoBehaviour {

    ICCardScanner[] m_CardScanners;

    private void Start()
    {
        m_CardScanners = FindObjectsOfType<ICCardScanner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var scanner in m_CardScanners)
            {
                scanner.CloseGates();
            }
        }
    }
    
}