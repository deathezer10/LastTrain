using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToggleTrainLights : MonoBehaviour
{
    List<Light> m_TrainLights = new List<Light>();
    public string SwitchCabinsName;

    public bool bIsOn = true;
    private float DefaultIntensity = 0.8f;
    private float RandomIntensity;
    private float RandomTime;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_TrainLights.Add(transform.GetChild(i).GetComponent<Light>());
        }
    }

    public void FlickerLights()
    {
        StartCoroutine(flicker());
    }

    private IEnumerator flicker()
    {
        RandomIntensity = Random.Range(0.2f, 0.4f);
        RandomTime = Random.Range(0.4f, 0.6f);
        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(RandomIntensity,RandomTime);
        }
        yield return new WaitForSeconds(RandomTime);

        
        RandomTime = Random.Range(0.5f, 0.7f);
        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(DefaultIntensity, RandomTime);
        }
        yield return new WaitForSeconds(RandomTime);

        RandomIntensity = Random.Range(0.4f, 0.55f);
        RandomTime = Random.Range(0.6f, 0.78f);
        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(RandomIntensity, RandomTime);
        }
        yield return new WaitForSeconds(RandomTime);

        RandomTime = Random.Range(0.7f, 0.8f);
        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(DefaultIntensity, RandomTime);
        }
        yield return true;
    }

    public void ToggleLights()
    {
        if(bIsOn)
        {
            bIsOn = false;
            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }
        }

        else
        {
            bIsOn = true;
            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(true);
            }
        }
    }
}
