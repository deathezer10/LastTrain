using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTrainLights : MonoBehaviour {
    List<Light> m_TrainLights = new List<Light>();
    public string SwitchCabinsName;

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

    public void LightsOn()
    {
        foreach (Light light in m_TrainLights)
        {
            light.gameObject.SetActive(true);
        }
    }

    public void LightsOff()
    {
        foreach (Light light in m_TrainLights)
        {
            light.gameObject.SetActive(false);
        }
    }

}
