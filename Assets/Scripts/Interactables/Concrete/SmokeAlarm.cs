using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAlarm : MonoBehaviour
{
    AudioPlayer fireAlarmSound;

    TrainDoorHandler trainDoorHandler;
    TrainDoorsOpenSound doorSound;
    bool alarmStarted;

    void Start()
    {
        fireAlarmSound = GetComponent<AudioPlayer>();
        trainDoorHandler = FindObjectOfType<TrainDoorHandler>();
        doorSound = FindObjectOfType<TrainDoorsOpenSound>();
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

                if (doorSound != null)
                    doorSound.CabinDoorsPlay();
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
                if (doorSound != null)
                    doorSound.CabinDoorsStopPlay();
            }

            alarmStarted = false;
        }
    }
}
