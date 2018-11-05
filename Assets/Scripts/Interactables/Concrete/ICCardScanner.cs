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
        FindObjectOfType<TutorialManager>().GetComponent<AudioPlayer>().Play("tutorial_finale");
    }

}
