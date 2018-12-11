using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTimeHandler : MonoBehaviour
{

    [SerializeField]
    private float m_TimeBetweenEachStation = 90;

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
            displayLight.ToggleLights(true, true,false);
        }

        m_CurrentAnnouncementCount = 0;
        StartCoroutine(CycleDisplayLights());
    }

    IEnumerator CycleDisplayLights()
    {
        while (m_CurrentAnnouncementCount < StationDisplayLight.STATIONNODE_AMOUNT)
        {
            yield return new WaitForSeconds(m_TimeBetweenEachStation);

            m_CurrentAnnouncementCount++;

            foreach (var displayLight in m_DisplayLights)
            {
                displayLight.CycleNodes();
            }
        }
    }

    public float GetTimeBetween()
    {
        return m_TimeBetweenEachStation;
    }
    
}
