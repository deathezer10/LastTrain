using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTimeHandler : MonoBehaviour {

    float m_CurrentTrainTime = 0;
    int m_CurrentAnnouncementCount = 0;

    StationDisplayLight[] m_DisplayLights;

    private void Start()
    {
        m_DisplayLights = FindObjectsOfType<StationDisplayLight>();
    }

    public void StartTrainTime()
    {
        foreach (var displayLight in m_DisplayLights)
        {
            displayLight.ToggleLights(true, true);
            StartCoroutine(CycleDisplayLights());
        }
    }

    public void StopTrainTime()
    {
        StopAllCoroutines();

        // Victory here
    }

    IEnumerator CycleDisplayLights()
    {
        while (m_CurrentAnnouncementCount < 5)
        {
            m_CurrentAnnouncementCount++;

            foreach (var displayLight in m_DisplayLights)
            {
                displayLight.CycleNodes();
            }

            // TODO play announcement
            
            yield return new WaitForSeconds(60);
        }


    }

    private void Update()
    {
        m_CurrentTrainTime += Time.deltaTime;


    }

}
