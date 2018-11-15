using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class StationChangedEvent : UnityEvent<int, string, string>
{
}

public class StationDisplayLight : MonoBehaviour
{

    public static int STATIONNODE_AMOUNT = 5;

    List<Transform> m_StationNodes = new List<Transform>();

    MeshRenderer m_MeshRenderer;

    int m_CurrentNodeIndex = 0;

    bool m_IsLightBlinking = false;

    TextMeshPro StationNameEnTextMeshP, StationNameJpTextMeshP;

    string[] stationEnNameArray = { "Furuhashi", "Shin-Kita", "Maiyama", "Nakashino", "Murihari", "Death" };
    string[] stationJpNameArray = { "bo ZL", "LtGP", "eBje", "UFLY", "gnZn", "LX" };

    /// <summary>
    /// Callback when the station has changed
    /// <para>Arg0: New station index starting from 0</para>
    /// <para>Arg1: New station name in english</para>
    /// <para>Arg2: New station name in japanese</para>
    /// </summary>
    public StationChangedEvent OnStationChanged = new StationChangedEvent();

    private void Start()
    {
        for (int i = 0; i < transform.parent.childCount; ++i)
        {
            Transform child = transform.parent.GetChild(i);

            if (child.name.Contains("Node"))
            {
                m_StationNodes.Add(child);
            }
            else if (child.name.Contains("DisplayEN"))
            {
                StationNameEnTextMeshP = child.gameObject.GetComponent<TextMeshPro>();
            }
            else if (child.name.Contains("DisplayJP"))
            {
                StationNameJpTextMeshP = child.gameObject.GetComponent<TextMeshPro>();
            }
        }

        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();

        transform.position = m_StationNodes[0].position;

        StationNameEnTextMeshP.text = stationEnNameArray[1];
        StationNameJpTextMeshP.text = stationJpNameArray[1];

        ToggleLights(true, false);
    }

    public void CycleNodes()
    {
        m_CurrentNodeIndex++;

        if (m_CurrentNodeIndex >= m_StationNodes.Count)
            m_CurrentNodeIndex = m_StationNodes.Count - 1;

        int nextNode = m_CurrentNodeIndex + 1;

        if (nextNode > STATIONNODE_AMOUNT)
            nextNode = STATIONNODE_AMOUNT;

        transform.position = m_StationNodes[m_CurrentNodeIndex].position;

        StationNameEnTextMeshP.text = stationEnNameArray[nextNode];
        StationNameJpTextMeshP.text = stationJpNameArray[nextNode];

        m_MeshRenderer.enabled = true;

        transform.parent.GetComponentInParent<StationDisplayUpdateSound>().PlayUpdateSound();

        OnStationChanged.Invoke(m_CurrentNodeIndex, StationNameEnTextMeshP.text, StationNameJpTextMeshP.text);
    }

    public void ToggleLights(bool enabled, bool enableBlink)
    {
        if (enabled)
        {
            m_MeshRenderer.enabled = true;

            if (enableBlink && !m_IsLightBlinking)
            {
                StartCoroutine(BlinkLight());
            }
        }
        else
        {
            m_IsLightBlinking = false;
            m_MeshRenderer.enabled = false;
            StopCoroutine(BlinkLight());
        }

        m_IsLightBlinking = enableBlink;

        if (enableBlink == false)
            StopAllCoroutines();
    }

    IEnumerator BlinkLight()
    {
        const float m_VisibleTime = 0.75f;
        const float m_InvisibleTime = 0.5f;

        while (true)
        {
            if (m_MeshRenderer.enabled)
            {
                m_MeshRenderer.enabled = false;
                yield return new WaitForSeconds(m_InvisibleTime);
            }
            else
            {
                m_MeshRenderer.enabled = true;
                yield return new WaitForSeconds(m_VisibleTime);
            }
        }
    }

}
