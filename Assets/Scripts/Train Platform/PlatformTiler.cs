using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTiler : MonoBehaviour
{

    public GameObject m_TunnelPrefab;

    GameObject m_TrainStation;

    int m_InitialTunnelSpawnAmount = 3;

    const float m_TunnelGapOffset = 20.05f;

    private void Start()
    {
        for (int i = 1; i <= m_InitialTunnelSpawnAmount; ++i)
        {
            Instantiate(m_TunnelPrefab, new Vector3(0, 0, i * m_TunnelGapOffset), Quaternion.identity, transform);
            Instantiate(m_TunnelPrefab, new Vector3(0, 0, i * -m_TunnelGapOffset), Quaternion.identity, transform);
        }
    }

    public void StartTiling()
    {
        StartCoroutine(BeginTiling());
    }

    private IEnumerator BeginTiling()
    {
        while (true)
        {
            m_TrainStation.transform.Translate(Vector3.back * Time.deltaTime);

            yield return null;
        }
    }

}
