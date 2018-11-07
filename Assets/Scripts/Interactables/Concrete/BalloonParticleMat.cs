using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonParticleMat : MonoBehaviour
{
    public void SetParticleColor(Color _matColor)
    {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().material.color = _matColor;
        GetComponent<AudioPlayer>().Play();
    }
}
