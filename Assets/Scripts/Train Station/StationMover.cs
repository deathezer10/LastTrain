using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationMover : MonoBehaviour
{

    // Tiling variables
    public GameObject m_TunnelPrefab;
    GameObject m_LastRightTunnel;
    Queue<Transform> m_InitialRemovableObjects = new Queue<Transform>();
    Queue<Transform> m_RemovableObjects = new Queue<Transform>();
    const int m_InitialTunnelSpawnAmount = 7;
    int m_CurrentTunnelIndex = 0;
    float m_CurrentDistanceTraveled = 0;
    const float m_TunnelGapOffset = 20.23f;
    const float m_TunnelXOffset = -5.05f;
    bool m_IsFirstTimeDestroy = true;
    public TrainDoorsOpenSound trainSounds;
    // Movement variables
    bool m_IsMoving = false;
    public bool isMoving {
        get { return m_IsMoving; }
    }

    float m_CurrentStationMaxSpeed = 20;
    public float currentMaxSpeed {
        get { return m_CurrentStationMaxSpeed; }
        set { m_CurrentStationMaxSpeed = value; }
    }

    const float m_StationAcceleration = 2.0f;
    float m_CurrentStationSpeed = 0;
    public float currentSpeed {
        get { return m_CurrentStationSpeed; }
        set { m_CurrentStationSpeed = value;}
    }

    private void Start()
    {
        for (int i = 3; i <= m_InitialTunnelSpawnAmount; ++i)
        {
            m_LastRightTunnel = Instantiate(m_TunnelPrefab, new Vector3(m_TunnelXOffset, 0, i * m_TunnelGapOffset), Quaternion.identity, transform); // Right side
            GameObject leftTunnel = Instantiate(m_TunnelPrefab, new Vector3(m_TunnelXOffset, 0, i * -m_TunnelGapOffset), Quaternion.identity, transform); // Left Side
            m_InitialRemovableObjects.Enqueue(m_LastRightTunnel.transform);
            m_InitialRemovableObjects.Enqueue(leftTunnel.transform);
        }

        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).tag != "TrainTunnel")
                m_InitialRemovableObjects.Enqueue(transform.GetChild(i));
        }

        trainSounds = FindObjectOfType<TrainDoorsOpenSound>();
    }

    private void Update()
    {
        m_CurrentStationSpeed = Mathf.Clamp(m_CurrentStationSpeed + (m_StationAcceleration * ((m_IsMoving) ? 1 : -1) * Time.deltaTime), 0, m_CurrentStationMaxSpeed);
        m_CurrentDistanceTraveled += m_CurrentStationSpeed * Time.deltaTime;
        trainSounds.SetAudioLevelSpeed(m_CurrentStationSpeed);
        transform.Translate(Vector3.back * m_CurrentStationSpeed * Time.deltaTime);

        if (Mathf.Abs(m_CurrentDistanceTraveled) >= m_TunnelGapOffset)
        {
            m_CurrentDistanceTraveled = 0;
            m_CurrentTunnelIndex++;

            // For the first time after spawning X tunnels, destroy all the initial objects
            if (m_IsFirstTimeDestroy && m_CurrentTunnelIndex >= m_InitialTunnelSpawnAmount * 2)
            {
                while (m_InitialRemovableObjects.Count > 0)
                {
                    Destroy(m_InitialRemovableObjects.Dequeue().gameObject);
                }

                m_IsFirstTimeDestroy = false;
                m_CurrentTunnelIndex = 0;
            }
            else if (!m_IsFirstTimeDestroy && m_CurrentTunnelIndex >= 3)
            {
                Destroy(m_RemovableObjects.Dequeue().gameObject);
            }

            m_LastRightTunnel = Instantiate(m_TunnelPrefab, new Vector3(m_TunnelXOffset, 0, m_LastRightTunnel.transform.position.z + m_TunnelGapOffset), Quaternion.identity, transform);
            m_RemovableObjects.Enqueue(m_LastRightTunnel.transform);
        }
    }

    public void ToggleMovement(bool moving)
    {
        m_IsMoving = moving;
    }

}
