using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ToggleTrainLights : MonoBehaviour
{
    List<Light> m_TrainLights = new List<Light>();
    public string SwitchCabinsName;

    public bool bIsOn = true;

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
        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(0.25f, 0.5f);
        }
        yield return new WaitForSeconds(0.5f);

        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(0.8f, 0.6f);
        }
        yield return new WaitForSeconds(0.6f);

        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(0.5f, 0.7f);
        }
        yield return new WaitForSeconds(0.7f);

        foreach (Light light in m_TrainLights)
        {
            light.DOIntensity(0.8f, 0.7f);
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
