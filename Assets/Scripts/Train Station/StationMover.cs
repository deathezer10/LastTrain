using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StationMover : MonoBehaviour
{

    #region Tiling variables
    GameObject m_LastRightTunnel;
    public GameObject m_DummyTrain;


    Queue<Transform> m_InitialRemovableObjects = new Queue<Transform>();
    Queue<Transform> m_RemovableObjects = new Queue<Transform>();

    const int m_InitialTunnelSpawnAmount = 7;
    int m_DestroySkipCounter = 3;

    float m_TunnelExtraGapOffset = 0;
    bool m_ExtraGapPending = false;

    const float m_TunnelGapOffset = 20.23f;
    const float m_TunnelXOffset = -5.05f;

    int m_CurrentTunnelIndex = 0;
    float m_CurrentDistanceTraveled = 0;
    bool m_IsFirstTimeDestroy = true;
    bool m_SpawnStationNext = false;
    private bool bSpawnDummyTrain = false;
    private bool bTrackDummyTrain = false;
    private bool bMoveDummyTrain = false;
    private bool bPlayOnce = true;
    private bool bSkip = true;

    private BoxCollider CrashChecker;

    [SerializeField]
    private GameObject m_TunnelPrefab;

    [SerializeField]
    private GameObject m_FakeStationPrefab;

    [SerializeField]
    private GameObject m_EmergencyExitPrefab;

    [SerializeField]
    private GameObject m_DummyTrainPrefab;

    [SerializeField]
    private TrainDoorsOpenSound trainSounds;

    [SerializeField]
    private StationDisplayLight m_StationDisplayLight;
    #endregion

    #region Movement Variables
    bool m_IsStopping = false;

    bool m_IsMoving = false;
    public bool isMoving {
        get { return m_IsMoving; }
        set { m_IsMoving = value; }
    }

    float m_CurrentStationMaxSpeed = 20;
    public float currentMaxSpeed {
        get { return m_CurrentStationMaxSpeed; }
        set { m_CurrentStationMaxSpeed = value; }
    }

    public float m_StationAcceleration = 2.0f;

    float m_CurrentStationSpeed = 0;
    public float currentSpeed {
        get { return m_CurrentStationSpeed; }
        set { m_CurrentStationSpeed = value; }
    }
    #endregion

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

        m_StationDisplayLight.OnStationChanged.AddListener(OnStationChanged);
        CrashChecker = FindObjectOfType<TrainCrashChecker>().GetComponent<BoxCollider>();

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            FindObjectOfType<TrainSpeedHandler>().ChangeSpeed(5);
            PrepareToStop();
        }

        m_CurrentStationSpeed = Mathf.Clamp(m_CurrentStationSpeed + (m_StationAcceleration * ((m_IsMoving) ? 1 : -1) * Time.deltaTime), 0, m_CurrentStationMaxSpeed);
        m_CurrentDistanceTraveled += m_CurrentStationSpeed * Time.deltaTime;
        trainSounds.SetAudioLevelPitch(m_CurrentStationSpeed);
        transform.Translate(Vector3.back * m_CurrentStationSpeed * Time.deltaTime);



        if (bTrackDummyTrain)
        {
            if (Vector3.Distance(CrashChecker.transform.position, m_DummyTrain.transform.position) < 85)
            {
                if (bPlayOnce)
                {
                    FindObjectOfType<DummyTrain>()?.HonkHorn();
                    bPlayOnce = false;
                }
            }

           

        }


        // Spawn a tunnel if train traveled more than a certain distance
        if (m_CurrentDistanceTraveled >= m_TunnelGapOffset)
        {
            m_CurrentDistanceTraveled = 0;
            m_CurrentTunnelIndex++;

            // For the first time after spawning X tunnels, destroy all the initially spawned objects
            if (m_IsFirstTimeDestroy && m_CurrentTunnelIndex >= m_InitialTunnelSpawnAmount * 2)
            {
                while (m_InitialRemovableObjects.Count > 0)
                {
                    var removable = m_InitialRemovableObjects.Dequeue();

                    if (removable != null)
                        Destroy(removable.gameObject);
                }

                m_IsFirstTimeDestroy = false;
                m_CurrentTunnelIndex = 0;
            }
            else if (!m_IsFirstTimeDestroy && m_CurrentTunnelIndex >= 3)
            {
                if (m_DestroySkipCounter > 0)
                {
                    m_DestroySkipCounter--;
                }
                else
                {
                    Destroy(m_RemovableObjects.Dequeue().gameObject);
                }
            }

            if (m_IsStopping)
            {
                m_LastRightTunnel = Instantiate(m_EmergencyExitPrefab, new Vector3(m_TunnelXOffset, 0, m_LastRightTunnel.transform.position.z + m_TunnelGapOffset + m_TunnelExtraGapOffset), Quaternion.identity, transform);
                m_RemovableObjects.Enqueue(m_LastRightTunnel.transform);
                m_IsStopping = false;

                if (m_ExtraGapPending)
                {
                    m_TunnelExtraGapOffset = 0;
                    m_ExtraGapPending = false;
                }
            }
            else
            {
                // Spawn a fake train station for the upcoming tunnel
                if (m_SpawnStationNext)
                {
                    m_SpawnStationNext = false;
                    m_LastRightTunnel = Instantiate(m_FakeStationPrefab, new Vector3(0, 0, m_LastRightTunnel.transform.position.z + (m_TunnelGapOffset * 3) + m_TunnelExtraGapOffset), Quaternion.identity, transform);

                    if (bSpawnDummyTrain)
                    {
                        m_DummyTrain = m_LastRightTunnel.transform.Find("DummyTrain").gameObject;
                        m_DummyTrain.gameObject.SetActive(true);
                        bTrackDummyTrain = true;
                    }
                    else
                    {
                        m_TunnelExtraGapOffset += m_TunnelGapOffset * 2; // Offset the next tunnel to account for the length of the train station
                        m_DestroySkipCounter += 2;
                        m_ExtraGapPending = true;
                    }

                    m_RemovableObjects.Enqueue(m_LastRightTunnel.transform);
                }
                else
                {
                    m_LastRightTunnel = Instantiate(m_TunnelPrefab, new Vector3(m_TunnelXOffset, 0, m_LastRightTunnel.transform.position.z + m_TunnelGapOffset + m_TunnelExtraGapOffset), Quaternion.identity, transform);
                    m_RemovableObjects.Enqueue(m_LastRightTunnel.transform);

                    if (m_ExtraGapPending)
                    {
                        m_TunnelExtraGapOffset = 0;
                        m_ExtraGapPending = false;
                    }
                }
            }

        }
    }

    public void ToggleMovement(bool moving)
    {
        m_IsMoving = moving;
    }

    public void PrepareToStop()
    {
        m_IsStopping = true;
    }

    private void OnStationChanged(int stationNumber, string stationNameEN, string stationNameJP)
    {

        if (!bSkip)
            if (stationNumber == 4)
            {
                if (currentSpeed == 0)
                {
                    m_DummyTrain = Instantiate(m_DummyTrainPrefab, m_LastRightTunnel.transform.position, m_DummyTrainPrefab.transform.rotation);
                    m_DummyTrain.transform.Rotate(new Vector3(0,180,0));
                    m_DummyTrain.transform.DOMoveZ(CrashChecker.transform.position.z - 40, 10f, false);
                    bTrackDummyTrain = true;
                   

                }

                else
                    bSpawnDummyTrain = true;
            }

        if (stationNumber == 4 && bSkip)
        {
            bSkip = false;
        }



        m_SpawnStationNext = true;
    }
}
