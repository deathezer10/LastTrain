﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSpeedHandler : MonoBehaviour
{
    private StationMover stationMover;
    private TrainDoorsOpenSound trainDoorsOpenSound;
    public bool bIsBrakeEngaged = false;
    public bool bIsAccelerator0 = false;

    private float PreviousTrainSpeed = 10.0f;
    private float NewTrainSpeed;
    private float rate = 0.1f;
    private float i = 0;

    private bool bBr_StopTrain = false;
    private bool bBr_SlowDown = false;
    private bool bAc_StopTrain = false;
    private bool bAc_SlowDownTrain = false;
    private bool bAc_SpeedChange = false;

    public bool bCanAccelerate { get; private set; } = true;

    void Start()
    {
        stationMover = FindObjectOfType<StationMover>();
        trainDoorsOpenSound = FindObjectOfType<TrainDoorsOpenSound>();
    }

    public void BrakeStop()
    {
        bBr_StopTrain = true;
        PreviousTrainSpeed = stationMover.currentSpeed;
        i = 0;
    }

    public void BrakeSlowDown()
    {
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
        NewTrainSpeed = Mathf.Lerp(3, 10,val );
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
            if (stationMover.currentSpeed == 0)
            {
                bBr_StopTrain = false;
                i = 0;
                bCanAccelerate = false;
            }
        }

        else if (bBr_SlowDown)
        {
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 3, i);
            if (stationMover.currentSpeed == 3)
            {
                bBr_SlowDown = false;
                i = 0;
            }
        }

        if (bAc_StopTrain)
        {
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 0, i);
            if (stationMover.currentSpeed == 0)
            {
                bAc_StopTrain = false;
                bCanAccelerate = false;
            }
        }


        else if(bAc_SlowDownTrain)
        {
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 3, i);
            if (stationMover.currentSpeed == 3)
            {
                bAc_SlowDownTrain = false;
                i = 0;
            }
        }


        if(bAc_SpeedChange)
        {
            i += Time.deltaTime * rate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i);
            
            if(stationMover.currentSpeed == NewTrainSpeed)
            {
                bAc_SpeedChange = false;
            }
        }
    }

}
