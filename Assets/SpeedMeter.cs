﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpeedMeter : MonoBehaviour
{


    private float DefaultRotation;
    private float timeAtSameSpeed;
    private float previousSpeed = -999;
    private bool bLeftSwivel = false;
    private bool bRightSwivel = false;


    [SerializeField]
    private float MaxRotation;

    private Quaternion lastRotation;

    private StationMover stationMover;

    // Use this for initialization
    void Start()
    {
        DefaultRotation = transform.localRotation.eulerAngles.y;
        stationMover = FindObjectOfType<StationMover>();
    }

    private void Update()
    {
        if (transform.gameObject.name == "SpeedMeter_Pointer")
        {
            if (previousSpeed == stationMover.currentSpeed)
            {
                timeAtSameSpeed += Time.deltaTime;
            }



            else
            {
                timeAtSameSpeed = 0;
                bLeftSwivel = false;
                bRightSwivel = false;
            }
        }

        else if(stationMover.currentSpeed >= 4)
        {
            timeAtSameSpeed += Time.deltaTime;
        }
         
        else
        {
            timeAtSameSpeed = 0;
            bLeftSwivel = false;
            bRightSwivel = false;
        }


        if (timeAtSameSpeed > 0.25f)
        {
            var randomOats = Mathf.RoundToInt(Random.Range(0, 1.8f));
            
            if (randomOats == 0)
            {
                bLeftSwivel = true;
                bRightSwivel = false;
            }

            else
            {
                bRightSwivel = true;
                bLeftSwivel = false;
            }

        }

        if (transform.gameObject.name == "SpeedMeter_Pointer")
        {

            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.Lerp(DefaultRotation, MaxRotation, normalize01(stationMover.currentSpeed, 0, 20)), transform.localRotation.eulerAngles.z);
            if (stationMover.currentSpeed == 0) return;
            if (bLeftSwivel)
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - 0.5f, transform.localRotation.eulerAngles.z);

            else if (bRightSwivel)
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 0.5f, transform.localRotation.eulerAngles.z);
            previousSpeed = stationMover.currentSpeed;


        }


        else
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, Mathf.Lerp(DefaultRotation, MaxRotation,normalize01(stationMover.currentSpeed,0,20) *3), transform.localRotation.eulerAngles.z);

             
            if (bLeftSwivel)
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y - 0.5f, transform.localRotation.eulerAngles.z);

            if (bRightSwivel)
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y + 0.5f, transform.localRotation.eulerAngles.z);

            previousSpeed = stationMover.currentSpeed;
            lastRotation = transform.localRotation;

        }
    }


    public float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }

    private float normalize01(float value, float min, float max)
    {
        float normalized = (value - min) / (max - min);
        return normalized;
    }

}
