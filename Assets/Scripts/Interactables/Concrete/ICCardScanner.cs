using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICCardScanner : MonoBehaviour {
    
    bool m_ScannerBypassed = false;

    private void OnTriggerEnter(Collider other)
    {
        ICCard wallet = other.GetComponent<ICCard>();

        if (wallet != null && m_ScannerBypassed == false)
        {
            m_ScannerBypassed = true;
            OpenGates();
        }
    }

    public void OpenGates()
    {
        var tManager = FindObjectOfType<TutorialManager>();
        tManager.GetComponent<AudioPlayer>().Play("tutorial_finale");
        tManager.m_ImageUnlockGates.MoveToHolder();
        // TODO change scanner color and rotate the gates
    }

}
