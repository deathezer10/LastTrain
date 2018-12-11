using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrainSpeedHandler : MonoBehaviour
{
    private StationMover stationMover;
    private TrainDoorsOpenSound trainDoorsOpenSound;
    private StationDisplayLight[] displayLights;
  
    private float PreviousTrainSpeed = 20.0f;
    private float NewTrainSpeed;
    private float acceleratorRate = 0.05f;
    private float brakeRate = 0.2f;
    private float i = 0;

    private bool bBr_StopTrain = false;
    private bool bAc_SpeedChange = false;
    private bool bOnce = false;
    private AudioPlayer audioscreech;
    private Action Part1Complete;


    void Start()
    {
        stationMover = FindObjectOfType<StationMover>();
        trainDoorsOpenSound = FindObjectOfType<TrainDoorsOpenSound>();
        audioscreech = GetComponent<AudioPlayer>();
        Part1Complete += PlayPart2;
        displayLights = FindObjectsOfType<StationDisplayLight>();
    }

    public void BrakeStop() //This is called from brake lever when brake lever is activated
    {
        audioscreech.Play("screech1", Part1Complete, 0); 
        bBr_StopTrain = true;
        PreviousTrainSpeed = stationMover.currentSpeed;
        i = 0;
    }


    public void ChangeSpeed(float val) //Called from accelerator is moved
    {
        if (bBr_StopTrain) return;

        if(val > stationMover.currentMaxSpeed) stationMover.currentMaxSpeed = val;



        PreviousTrainSpeed = stationMover.currentSpeed;
        NewTrainSpeed = val;
        i = 0.0f;
        bAc_SpeedChange = true;
    }

    public void SetSpeedChanged(bool value)
    {
        bAc_SpeedChange = value;
    }

    void Update()
    {

        if (bBr_StopTrain)
        {
            i += Time.deltaTime * brakeRate;
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, 0, i);
            trainDoorsOpenSound.SetWindAudioLevel(stationMover.currentSpeed);

            if (!bOnce)
                if (stationMover.currentSpeed < 3.5f)
                {
                    bOnce = true;
                    audioscreech.Stop();
                    audioscreech.audioSource.loop = false;
                    audioscreech.audioSource.pitch = 1.0f;
                    audioscreech.Play("screech3");
                }

            if (stationMover.currentSpeed == 0)
            {
                audioscreech.Stop();
                i = 0;
                stationMover.currentMaxSpeed = 0;
                StopBlinkBlink();
            }
        }


        if (bAc_SpeedChange)
        {
            if(bBr_StopTrain)
            {
                bAc_SpeedChange = false;
                return;
            }
            
            
            i += Time.deltaTime * acceleratorRate;
            print(i);
            print(Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i));
            stationMover.currentSpeed = Mathf.Lerp(PreviousTrainSpeed, NewTrainSpeed, i);
            trainDoorsOpenSound.SetWindAudioLevel(stationMover.currentSpeed);

            if (stationMover.currentSpeed == NewTrainSpeed)
            {
                stationMover.currentMaxSpeed = stationMover.currentSpeed;
                PreviousTrainSpeed = stationMover.currentSpeed;
                bAc_SpeedChange = false;
            }
        }
    }

    private void PlayPart2()
    {
        audioscreech.audioSource.pitch = 0.8f;
        audioscreech.audioSource.loop = true;
        audioscreech.Play("screech2");
    }


  public void StopBlinkBlink()
    {
       for(int lights = 0; lights < displayLights.Length; lights++)
        {
            displayLights[lights].ToggleLights(false, false,true);
        }
    }

}
