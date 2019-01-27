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
    private TutorialGrabbableObject _wallet;

    [SerializeField]
    private TutorialGrabbableObject _card;

    [SerializeField]
    private TutorialObject _examination;

    public void Start()
    {
        // テレポートポイント
        _tereportPoint.IsEnterRP
            .Where(_ => _)
            .Select(_ => _tereportPoint)
            .Subscribe(_ => _.MarkerObject.SetActive(false));

        // 財布開けるまで
        _wallet.IsUseRP
            .Where(_ => _)
            .Select(_ => _wallet)
            .Subscribe(_ => _.MarkerObject.SetActive(false));

        // カード持っている間
        _card.IsGrabeRP
            .Where(_ => _)
            .Select(_ => _card)
            .Subscribe(_ => _.MarkerObject.SetActive(false));

        // カード持っていない間
        _card.IsGrabeRP
            .Where(_ => !_)
            .Select(_ => _card)
            .Subscribe(_ => _.MarkerObject.SetActive(true));

        // 改札当たったとき
        _examination.IsEnterRP
            .Where(_ => _)
            .Select(_ => _examination)
            .Subscribe(_ => _.MarkerObject.SetActive(false));
    }

}
