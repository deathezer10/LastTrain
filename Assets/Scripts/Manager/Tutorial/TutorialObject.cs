using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class TutorialObject : MonoBehaviour
{
    public GameObject MarkerObject { get; set; }

    public BoolReactiveProperty IsEnterRP { get; private set; } = new BoolReactiveProperty(false);

    private void OnTriggerEnter(Collider other)
    {
        IsEnterRP.Value = true;
    }
}
