﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scissors : GrabbableObject
{
    [SerializeField]
    Transform[] scissorSides;

    [SerializeField]
    GameObject cutObject;

    Vector3[] originalRots = {new Vector3(0, 0, 0), new Vector3(0, 180, 0) };
    Vector3[] turnToRots = { new Vector3(-15f, 0, 0), new Vector3(-15f, 180, 0) };

    float lastCutTimer;
    float cutTime = 0.4f;
    bool held, open;

    AudioPlayer cutAudio;


    private void Start()
    {
        cutObject.SetActive(false);
        cutAudio = GetComponent<AudioPlayer>();
    }

    public override void OnGrab()
    {
        base.OnGrab();

        held = true;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        held = false;
    }

    public override void OnUse()
    {
        base.OnUse();

        if (held && lastCutTimer >= Time.time && open)
        {
            open = false;

            lastCutTimer = Time.time + cutTime + 0.1f;

            cutAudio.Play();

            StartCoroutine(ScissorsCut());
        }
        else if (held && lastCutTimer >= Time.time && !open)
        {
            open = true;

            lastCutTimer = Time.time + cutTime + 0.1f;

            ScissorsOpen();
        }
    }

    private IEnumerator ScissorsCut()
    {
        int index = 0;
        scissorSides[index].DOLocalRotate(turnToRots[index], cutTime, RotateMode.Fast);
        index++;
        scissorSides[index].DOLocalRotate(turnToRots[index], cutTime, RotateMode.Fast);

        yield return new WaitForSeconds(cutTime / 5);

        cutObject.SetActive(true);

        yield return new WaitForSeconds(cutTime / 2);

        cutObject.SetActive(false);
    }

    private void ScissorsOpen()
    {
        int index = 0;
        scissorSides[index].DOLocalRotate(originalRots[index], cutTime, RotateMode.Fast);
        index++;
        scissorSides[index].DOLocalRotate(originalRots[index], cutTime, RotateMode.Fast);
    }

}
