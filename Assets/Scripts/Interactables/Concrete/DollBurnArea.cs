using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollBurnArea : MonoBehaviour
{

    ParticleSystem fireParticle, smokeParticle;

    float burnThreshold = 60f;
    float currentHeat;

    bool burning;

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

        GetComponentInParent<Doll>().BurningStart();
    }

    public void IncreaseHeat()
    {
        if (!burning)
        {
            currentHeat++;

            if (currentHeat >= burnThreshold)
            {
                burning = true;
                GetComponent<BoxCollider>().enabled = false;
                Burn();
            }
        }
    }
}
