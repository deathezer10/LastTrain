using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTimeHandler : MonoBehaviour
{
    private StationMover stationMover;

    const float m_TimeBetweenEachStation = 90;

    int m_CurrentAnnouncementCount = 0;

    StationDisplayLight[] m_DisplayLights;

    private void Start()
    {
        m_DisplayLights = FindObjectsOfType<StationDisplayLight>();
        stationMover = FindObjectOfType<StationMover>();
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
            // TODO play announcement
        }
        stationMover.OnShouldSpawnDummyTrain();
        //GameObject.FindWithTag("Player").GetComponent<PlayerDeathHandler>().KillPlayer("death_timeup");
    }


}
