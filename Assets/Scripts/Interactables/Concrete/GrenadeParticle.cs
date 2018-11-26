using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeParticle : MonoBehaviour
{

    void Start()
    {
        GetComponent<AudioPlayer>().Play();
    }
}
