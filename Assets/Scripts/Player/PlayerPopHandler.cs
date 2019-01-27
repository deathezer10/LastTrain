using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopHandler : MonoBehaviour
{
    [SerializeField]
    private Transform _initPopPos = null;

    [SerializeField]
    private CheckpointManager _checkpointManager = null;

    void Start()
    {
        var currentPopPos = _checkpointManager.GetCurrentPoint();
        currentPopPos = currentPopPos ?? _initPopPos;

        this.transform.position = currentPopPos.position;
        this.transform.eulerAngles = currentPopPos.eulerAngles;
    }
}
