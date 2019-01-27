using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class TutorialObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;

    [SerializeField]
    private Subject<bool> _tutorialSubject = new Subject<bool>();

    private void Start()
    {
        _tutorialSubject.Subscribe(_ =>
        {
            _object.SetActive(_);
        });
    }
}
