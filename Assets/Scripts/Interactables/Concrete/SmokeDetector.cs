using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDetector : MonoBehaviour , IShootable
{
    public Material[] indicatorMatArray;

    MeshRenderer indicatorRenderer;
    
    int currentTriggeredIndex;
    float currentSmoke;
    bool detectingSmoke, alarmTriggered;

    private void Start()
    {
        indicatorRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!alarmTriggered && detectingSmoke)
        {
            currentSmoke++;

            if (currentSmoke >= 60)
            {
                currentSmoke = 0;
                TriggerNextIndicator();
            }
        }
    }

    private void TriggerNextIndicator()
    {
        indicatorRenderer.material = indicatorMatArray[currentTriggeredIndex];

        currentTriggeredIndex++;

        if (currentTriggeredIndex >= indicatorMatArray.Length)
        {
            TriggerAlarm();
        }
    }

    public void DetectingSmoke(bool _smokeDetected)
    {
        detectingSmoke = _smokeDetected;
    }

    private void TriggerAlarm()
    {
        alarmTriggered = true;
        FindObjectOfType<SmokeAlarm>().StartSmokeAlarm();
    }

    public void OnShot(Revolver revolver)
    {
        TriggerAlarm();
    }
}
