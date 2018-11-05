using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : GrabbableObject
{

    public override void OnGrab()
    {
        TutorialManager tManager = FindObjectOfType<TutorialManager>();
        tManager.GetComponent<AudioPlayer>().Play("tutorial_wallet_outro");
    }
    
}
