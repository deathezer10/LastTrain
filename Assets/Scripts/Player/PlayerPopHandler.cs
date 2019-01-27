using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopHandler : MonoBehaviour
{

    [SerializeField]
    private Transform _initPopPos = null;

    void Start()
    {
        var currentPopPos = CheckpointManager.Instance.GetCurrentPoint();
        currentPopPos = currentPopPos ?? _initPopPos;

        this.transform.position = currentPopPos.position;
        this.transform.eulerAngles = currentPopPos.eulerAngles;
    }
}
