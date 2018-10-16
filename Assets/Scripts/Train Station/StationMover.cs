using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationMover : MonoBehaviour
{

    public GameObject m_TunnelPrefab;

    GameObject m_TrainStation;

    int m_InitialTunnelSpawnAmount = 3;

    const float m_TunnelGapOffset = 20.05f;

    bool m_IsMoving = false;

    const float m_StationMaxSpeed = 10;
    const float m_StationAcceleration = 1;
    float m_CurrentStationSpeed = 0;

    private void Start()
    {
        for (int i = 1; i <= m_InitialTunnelSpawnAmount; ++i)
        {
            Instantiate(m_TunnelPrefab, new Vector3(0, 0, i * m_TunnelGapOffset), Quaternion.identity, transform);
            Instantiate(m_TunnelPrefab, new Vector3(0, 0, i * -m_TunnelGapOffset), Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        if (m_IsMoving)
        {
            m_CurrentStationSpeed = Mathf.Clamp(m_CurrentStationSpeed + (m_StationAcceleration * Time.deltaTime), 0, m_StationMaxSpeed);
            m_TrainStation.transform.Translate(Vector3.back * m_CurrentStationSpeed * Time.deltaTime);
        }
        else
        {

        }
    }

    public void ToggleMovement(bool moving)
    {
        m_IsMoving = moving;

        if (moving)
        {
            StartCoroutine(BeginTiling());
        }
        else
        {
            StopCoroutine(BeginTiling());
        }
    }

    private IEnumerator BeginTiling()
    {
        while (true)
        {
            yield return null;
        }
    }

}
