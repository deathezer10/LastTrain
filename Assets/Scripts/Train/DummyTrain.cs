using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTrain : MonoBehaviour {

    public void HonkHorn()
    {
        GetComponent<AudioPlayer>().Play("horn");
    }
   
    public void CrashSound()
    {
        GetComponent<AudioPlayer>().Play("crash");
    }

}
