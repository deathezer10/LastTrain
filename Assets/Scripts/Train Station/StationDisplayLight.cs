using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StationDisplayLight : MonoBehaviour
{

    public static int STATIONNODE_AMOUNT = 4;

    List<Transform> m_StationNodes = new List<Transform>();

    MeshRenderer m_MeshRenderer;

    int m_CurrentNodeIndex = 0;

    bool m_IsLightBlinking = false;

    TextMeshPro stationNameMesh;

    string[] stationEnNameArray = { "Furuihashi", "Kiba", "Maiyama", "Nagashino", "Muridecho" };

    private void Start()
    {
        for (int i = 0; i < transform.parent.childCount; ++i)
        {
            Transform child = transform.parent.GetChild(i);

            if (child.name.Contains("Node"))
            {
                m_StationNodes.Add(child);
            }
        }

        stationNameMesh = transform.parent.GetComponentInChildren<TextMeshPro>();

        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();

        transform.position = m_StationNodes[0].position;

        stationNameMesh.text = stationEnNameArray[0];

        ToggleLights(true, false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CycleNodes();
        }
    }

    public void CycleNodes()
    {
        m_CurrentNodeIndex++;

        if (m_CurrentNodeIndex >= m_StationNodes.Count)
            m_CurrentNodeIndex = 0;

        transform.position = m_StationNodes[m_CurrentNodeIndex].position;

        stationNameMesh.text = stationEnNameArray[m_CurrentNodeIndex];

        m_MeshRenderer.enabled = true;

        transform.parent.GetComponentInParent<StationDisplayUpdateSound>().PlayUpdateSound();
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
