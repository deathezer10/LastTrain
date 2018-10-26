using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSetting : MonoBehaviour
{

    [SerializeField]
    private int fps = 60;

    [SerializeField]
    private bool isDebug = false;

    private void Awake()
    {
        Application.targetFrameRate = fps;
    }

    private void Update()
    {
        if (!isDebug) return;
        float fps = 1f / Time.deltaTime;
        Debug.LogFormat("{0}fps", fps);
    }
}
