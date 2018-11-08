using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAlarm : MonoBehaviour
{
    AudioPlayer fireAlarmSound;

    TrainDoorHandler trainDoorHandler;

    bool alarmStarted;

    void Start()
    {
        fireAlarmSound = GetComponent<AudioPlayer>();
        trainDoorHandler = FindObjectOfType<TrainDoorHandler>();
    }
    
    public void StartSmokeAlarm()
    {
        if (!alarmStarted)
        {
            alarmStarted = true;
            fireAlarmSound.Play();

            if (!trainDoorHandler.bAreDoorsOpen)
            {
                trainDoorHandler.ToggleDoors(true);
            }
        }
    }

    public void StopSmokeAlarm()
    {
        if (alarmStarted)
        {
            fireAlarmSound.Stop();

            if (trainDoorHandler.bAreDoorsOpen)
            {
                trainDoorHandler.ToggleDoors(false);
            }

            alarmStarted = false;
        }
    }
}
