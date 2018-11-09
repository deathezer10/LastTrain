using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrainSpeedHandler : MonoBehaviour
{
    private StationMover stationMover;
    private TrainDoorsOpenSound trainDoorsOpenSound;
    public bool bIsBrakeEngaged = false;
    public bool bIsAccelerator0 = false;

    private float PreviousTrainSpeed = 10.0f;
    private float NewTrainSpeed;
    private float rate = 0.25f;
    private float i = 0;

    private bool bBr_StopTrain = false;
    private bool bBr_SlowDown = false;
    private bool bAc_StopTrain = false;
    private bool bAc_SpeedChange = false;
    private bool bOnce = false;
    public bool bCanAccelerate { get; private set; } = true;
    private AudioPlayer audioscreech;
    private Action Part1Complete;


    void Start()
    {
        stationMover = FindObjectOfType<StationMover>();
        trainDoorsOpenSound = FindObjectOfType<TrainDoorsOpenSound>();
        audioscreech = GetComponent<AudioPlayer>();
        Part1Complete += PlayPart2;

    }

    public void BrakeStop()
    {
        audioscreech.Play("screech1", Part1Complete, 0);
        bBr_StopTrain = true;
        PreviousTrainSpeed = stationMover.currentSpeed;
        i = 0;
    }

    public void BrakeSlowDown()
    {
        audioscreech.Play("screech1", Part1Complete, 0);
        bBr_SlowDown = true;
        PreviousTrainSpeed = stationMover.currentSpeed;
        i = 0;
    }

    public void AcceleratorStop()
    {
        bAc_StopTrain = true;
        PreviousTrainSpeed = stationMover.currentSpeed;
        i = 0;
    }



    public void ChangeSpeed(float val)
    {
        PreviousTrainSpeed = stationMover.currentSpeed;
        NewTrainSpeed = val;
        if (NewTrainSpeed > stationMover.currentMaxSpeed) NewTrainSpeed = stationMover.currentMaxSpeed;
        i = 0.0f;
        bAc_SpeedChange = true;
    }

    void Update()
    {

        if (bBr_StopTrain)
        {
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 0, i);
            trainDoorsOpenSound.SetAudioLevel(stationMover.currentSpeed);

            if (!bOnce)
                if (stationMover.currentSpeed < 0.5)
                {
                    bOnce = true;
                    audioscreech.Stop();
                    audioscreech.Play("screech3");
                }

            if (stationMover.currentSpeed == 0)
            {
                audioscreech.Stop();
                bBr_StopTrain = false;
                i = 0;
                bCanAccelerate = false;
                stationMover.currentMaxSpeed = 0;
            }
        }


        if (bBr_SlowDown)
        {
            if (bAc_StopTrain || bBr_StopTrain) return;
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 3, i);
            trainDoorsOpenSound.SetAudioLevel(stationMover.currentSpeed);
            if (stationMover.currentSpeed == 3)
            {
                stationMover.currentMaxSpeed = 3;
                bBr_SlowDown = false;
                i = 0;
            }
        }

        if (bAc_StopTrain)
        {
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 0, i);
            trainDoorsOpenSound.SetAudioLevel(stationMover.currentSpeed);

            if (!bOnce)
                if (stationMover.currentSpeed < 0.5)
                {
                    bOnce = true;
                    audioscreech.Stop();
                    audioscreech.Play("screech3");
                }

            if (stationMover.currentSpeed == 0)
            {
                audioscreech.Stop();
                bAc_StopTrain = false;
                bCanAccelerate = false;
                stationMover.currentMaxSpeed = 0;
            }
        }


        if (bAc_SpeedChange)
        {
            if (bAc_StopTrain || bBr_StopTrain) return;
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i);
            trainDoorsOpenSound.SetAudioLevel(stationMover.currentSpeed);

            if (stationMover.currentSpeed == NewTrainSpeed)
            {
                bAc_SpeedChange = false;
            }
        }
    }

    private void PlayPart2()
    {
        audioscreech.audioSource.loop = true;
        audioscreech.Play("screech2");
    }


}
