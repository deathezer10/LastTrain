using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class TutorialObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _markerObj;
    public GameObject MarkerObject { get { return _markerObj; } set { _markerObj = value; } }

    public BoolReactiveProperty IsEnterRP { get; private set; } = new BoolReactiveProperty(false);

    protected virtual void OnTriggerEnter(Collider other)
    {
        IsEnterRP.Value = true;
    }
}
