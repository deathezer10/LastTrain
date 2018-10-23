using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTimeHandler : MonoBehaviour {
    
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
        }

        m_CurrentAnnouncementCount = 0;
        StartCoroutine(CycleDisplayLights());
    }

    public void StopTrainTime()
    {
        StopAllCoroutines();

        // Victory here
    }

    IEnumerator CycleDisplayLights()
    {
        while (m_CurrentAnnouncementCount < StationDisplayLight.STATIONNODE_AMOUNT - 1)
        {
            yield return new WaitForSeconds(60);

            m_CurrentAnnouncementCount++;

            foreach (var displayLight in m_DisplayLights)
            {
                displayLight.CycleNodes();
            }

            // TODO play announcement
        }

        GameObject.FindWithTag("Player").GetComponent<PlayerDeathHandler>().KillPlayer("death_timeup");
    }
   

}
