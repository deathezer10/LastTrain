using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : GrabbableObject
{

    private bool m_HasAnnounced = false;

    public override void OnGrab()
    {
        if (!m_HasAnnounced)
        {
            TutorialManager tManager = FindObjectOfType<TutorialManager>();
            tManager.GetComponent<AudioPlayer>().Play("tutorial_wallet_outro");
            m_HasAnnounced = true;
        }
    }
    
}
