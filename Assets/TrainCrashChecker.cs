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

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.tag == "DummyTrain")
        {
          deathHandler.FadeTime = 1;
          deathHandler.KillPlayer("death_timeup");
        }

    }

}
