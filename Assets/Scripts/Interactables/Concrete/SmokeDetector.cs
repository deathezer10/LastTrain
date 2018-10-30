using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDetector : MonoBehaviour
{
    public MeshRenderer[] indicatorArray;
    public Color triggeredColor;

    AudioPlayer fireAlarmSound;
    int currentTriggeredIndex;
    float currentSmoke;
    bool detectingSmoke, alarmTriggered;

    private void Start()
    {
        fireAlarmSound = GetComponent<AudioPlayer>();
    }

    void Update()
    {
        if (!alarmTriggered && detectingSmoke)
        {
            currentSmoke++;

            if (currentSmoke >= 40)
            {
                currentSmoke = 0;
                TriggerNextIndicator();
            }
        }
    }

    private void TriggerNextIndicator()
    {
        indicatorArray[currentTriggeredIndex].material.color = triggeredColor;
        currentTriggeredIndex++;

        if (currentTriggeredIndex >= indicatorArray.Length)
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
        Debug.Log("Smoke alarm triggered, open doors.");
        alarmTriggered = true;
        // Call door open
        fireAlarmSound.Play();
    }
}
