using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAlarm : MonoBehaviour
{
    AudioPlayer fireAlarmSound;

    bool alarmStarted;

    void Start()
    {
        fireAlarmSound = GetComponent<AudioPlayer>();
    }
    
    public void StartSmokeAlarm()
    {
        if (!alarmStarted)
        {
            alarmStarted = true;
            fireAlarmSound.Play();
            // Call to open doors.
        }
    }
}
