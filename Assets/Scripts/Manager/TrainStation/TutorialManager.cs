using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;

    [SerializeField]
    public bool _tutorialEnabled { get; set; } = true;

    [SerializeField]
    private SteamVR_Action_Boolean _padAction;


    [SerializeField]
    Color m_EmissionHighlightColor;

    private Vector3 m_playerCheckpointPos = new Vector3(1f, 0.83f, -5f);
    private Vector3 m_playerCheckpointRot = new Vector3(0, -90f, 0);

    private GameObject m_RightDiscObject;
    private List<GameObject> m_TriggerObjects = new List<GameObject>(), m_GripObjects = new List<GameObject>();

    private Material m_InitialControllerMat;
    public Material m_HighlightControllerMat;


    void Start()
    {
        var checkpoint = CheckpointManager.Instance.CurrentPointNum;

        if (!_tutorialEnabled) CheckPointStart();


    }

    private void CheckPointStart()
    {
        if (_player)
        {
            _player.transform.position = m_playerCheckpointPos;
            _player.transform.eulerAngles = m_playerCheckpointRot;
        }
    }
}
