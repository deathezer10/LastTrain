﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElectricalBoxButton : MonoBehaviour
{

    private const float m_ToggleOffset = 0.02f;
    private AudioPlayer Audio;
    private bool bDisable = false;

    void Start()
    {
        Audio = transform.GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Controller"))
            if (!bDisable)
            {
                bDisable = true;
                Audio.Play();
                transform.DOLocalMoveX(m_ToggleOffset, 0.09f).SetRelative();
                FindObjectOfType<TrainDoorHandler>().ToggleDriverDoor();
                FindObjectOfType<TrainDoorsOpenSound>().DriverDoorPlay();

                FindObjectOfType<EmergencyDoorsManager>().EmergencyDoorsTriggered();
            }
    }

}

