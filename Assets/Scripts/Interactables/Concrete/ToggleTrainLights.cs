using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTrainLights : MonoBehaviour {
    List<Light> m_TrainLights = new List<Light>();
    public string SwitchCabinsName;
    bool bIsOn = true;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_TrainLights.Add(transform.GetChild(i).GetComponent<Light>());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FlickerLights()
    {
        StartCoroutine(flicker());
    }

    private IEnumerator flicker()
    {
        if(!bIsOn)
        {
            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(1);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(true);
            }
            yield return true;
        }

        if (bIsOn)
        {
            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(1);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }
            yield return true;
        }


        yield return true;
    }
    public void LightsOn()
    {
        foreach (Light light in m_TrainLights)
        {
            light.gameObject.SetActive(true);
            bIsOn = true;
        }
    }

    public void LightsOff()
    {
        foreach (Light light in m_TrainLights)
        {
            light.gameObject.SetActive(false);
            bIsOn = false;
        }
    }

}
