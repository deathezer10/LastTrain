using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationMover : MonoBehaviour
{

    // Tiling variables
    public GameObject m_Train;
    public GameObject m_TunnelPrefab;
    GameObject m_LastRightTunnel;
    int m_InitialTunnelSpawnAmount = 3;
    int m_CurrentTunnelIndex = 0;
    float m_CurrentDistanceTraveled = 0;
    const float m_TunnelGapOffset = 20.05f;

    // Movement variables
    bool m_IsMoving = false;
    const float m_StationMaxSpeed = 10;
    const float m_StationAcceleration = 1;
    float m_CurrentStationSpeed = 0;

    private void Start()
    {
        for (int i = 1; i <= m_InitialTunnelSpawnAmount; ++i)
        {
            m_CurrentTunnelIndex = i;
            Instantiate(m_TunnelPrefab, new Vector3(0, 0, i * m_TunnelGapOffset), Quaternion.identity, transform); // Right side
            Instantiate(m_TunnelPrefab, new Vector3(0, 0, i * -m_TunnelGapOffset), Quaternion.identity, transform); // Left Side
        }
    }

    private void Update()
    {
        m_CurrentStationSpeed = Mathf.Clamp(m_CurrentStationSpeed + (m_StationAcceleration * ((m_IsMoving) ? 1 : -1) * Time.deltaTime), 0, m_StationMaxSpeed);
        m_CurrentDistanceTraveled += m_CurrentStationSpeed * Time.deltaTime;

        transform.Translate(Vector3.back * m_CurrentStationSpeed * Time.deltaTime);

        if (m_CurrentDistanceTraveled >= m_TunnelGapOffset)
        {
            m_CurrentDistanceTraveled = 0;
            m_CurrentTunnelIndex++;

            GameObject tunnel = Instantiate(m_TunnelPrefab, new Vector3(0, 0, m_CurrentTunnelIndex * m_TunnelGapOffset), Quaternion.identity, transform);
            tunnel.gameObject.AddComponent<StationTileDestroyer>().SetUpDestroyer(m_Train, m_InitialTunnelSpawnAmount * m_TunnelGapOffset);
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
