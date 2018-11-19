using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDeathParticle : MonoBehaviour
{

    void Start()
    {
        GetComponent<AudioPlayer>().Play();
    }

}
