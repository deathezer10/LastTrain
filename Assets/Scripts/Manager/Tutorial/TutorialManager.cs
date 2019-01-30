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

    public bool _tutorialEnabled { get; set; } = true;

    [SerializeField]
    private TutorialObject _tereportPoint;

    [SerializeField]
    private TutorialGrabbableObject _wallet;

    [SerializeField]
    private TutorialGrabbableObject _card;

    [SerializeField]
    private TutorialObject[] _examinations;

    public void Start()
    {
        // テレポートポイント
        _tereportPoint?.IsEnterRP
            .Where(_ => _)
            .Select(_ => _tereportPoint)
            .Subscribe(_ =>
            {
                _.MarkerObject.SetActive(false);
                _wallet?.MarkerObject.SetActive(true);
            });


        // 財布開けるまで
        _wallet?.IsGrabeRP
          .Where(_ => _)
          .Select(_ => _wallet)
          .Subscribe(_ =>
          {
              _.MarkerObject.SetActive(false);
          });

        _wallet?.IsGrabeRP
           .Where(_ => !_)
           .Where(_ => !_card.gameObject.activeInHierarchy)
           .Select(_ => _wallet)
           .Where(_ => !_.IsUseRP.Value)
           .Where(_ => !_tereportPoint.gameObject.activeSelf)
           .Subscribe(_ =>
           {
               _.MarkerObject.SetActive(true);
           });

        _wallet?.IsUseRP
            .Where(_ => _)
            .Select(_ => _wallet)
            .Subscribe(_ =>
            {
                _.MarkerObject.SetActive(false);
                _card?.MarkerObject.SetActive(true);
            });

        // カード
        _card?.IsGrabeRP
          .Where(_ => _)
          .Where(_ => !ExaminationIsEnter())
          .Select(_ => _card)
          .Subscribe(_ =>
          {
              _.MarkerObject.SetActive(false);
              ExaminationAction(obj => obj.MarkerObject.SetActive(true));
          });

        _card?.IsGrabeRP
           .Where(_ => !_)
           .Where(_ => !ExaminationIsEnter())
           .Select(_ => _card)
           .Where(_ => !_.IsUseRP.Value)
           .Subscribe(_ =>
           {
               _.MarkerObject.SetActive(true);
               ExaminationAction(obj => obj.MarkerObject.SetActive(false));
           });

        // 改札当たったとき
        ExaminationAction(obj => obj.IsEnterRP
            .Where(_ => _)
            .Select(_ => obj)
            .Subscribe(_ => _.MarkerObject.SetActive(false)));
    }

    private void ExaminationAction(Action<TutorialObject> action)
    {
        foreach (var obj in _examinations)
        {
            action?.Invoke(obj);
        }
    }

    private bool ExaminationIsEnter()
    {
        foreach (var obj in _examinations)
        {
            if (obj.IsEnterRP.Value) return true;
        }
        return false;
    }
}
