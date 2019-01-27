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
    private TutorialObject _tereportPoint;

    [SerializeField]
    private TutorialObject _wallet;

    [SerializeField]
    private TutorialObject _card;

    [SerializeField]
    private TutorialObject _examination;

    public void Start()
    {

    }

}
