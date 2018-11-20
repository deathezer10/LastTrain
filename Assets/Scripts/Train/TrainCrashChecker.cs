using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCrashChecker : MonoBehaviour
{
    public PlayerDeathHandler deathHandler;
    // Use this for initialization
    void Start()
    {
       deathHandler = FindObjectOfType<PlayerDeathHandler>();
    }


    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "DummyTrain")
        {
           FindObjectOfType<DummyTrain>().CrashSound();
          deathHandler.FadeTime = 1;
          deathHandler.KillPlayer("death_timeup");
        }

    }

}
