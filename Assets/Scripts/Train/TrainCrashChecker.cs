using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCrashChecker : MonoBehaviour
{
    public PlayerDeathHandler deathHandler;

    void Start()
    {
       deathHandler = FindObjectOfType<PlayerDeathHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "DummyTrain")
        {
          deathHandler.m_DeathFader.SetFadeColor(Color.black);
          deathHandler.FadeTime = 1;
          deathHandler.KillPlayer("death_timeup");
        }

    }

}
