using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPBurnArea : MonoBehaviour
{
    ParticleSystem fireParticle, smokeParticle;

    float burnThreshold = 45f;
    float currentHeat;

    void Start()
    {
        fireParticle = GetComponent<ParticleSystem>();
        fireParticle.Stop();

        smokeParticle = GetComponentInChildren<ParticleSystem>();
        smokeParticle.Stop();
    }

    private void Burn()
    {
        fireParticle.Play();
        smokeParticle.Play();
    }

    public void IncreaseHeat()
    {
        currentHeat++;

        if (currentHeat >= burnThreshold)
        {
            GetComponent<BoxCollider>().enabled = false;
            Burn();
        }
    }
}
