﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Doll : GrabbableObject, IShootable
{
    public Transform head;
    public Material dollEyeMat;
    public Color initialColor, flashColor;

    Transform playerHeadTrans;

    Vector3 headInitRot;

    float flashEndTime;
    bool playerWithinRange;

    void Start()
    {
        playerHeadTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        dollEyeMat.SetColor("_EmissionColor", initialColor);
        headInitRot = head.localEulerAngles;
    }

    private void Update()
    {
        if (playerWithinRange)
        {
            head.LookAt(playerHeadTrans);
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.H))
        {
            StartEyeFlash(3f);
        }
    }

    /// <summary>
    /// Pass the duration of the announcement clip to the doll eye flasher
    /// </summary>
    /// <param name="_duration"></param>
    public void StartEyeFlash(float _duration)
    {
        flashEndTime = Time.time + _duration;
        StartCoroutine(EyeFlash());
    }

    private IEnumerator EyeFlash()
    {
        bool flash = false;

        while (Time.time <= flashEndTime)
        {
            if (!flash)
            {
                dollEyeMat.SetColor("_EmissionColor", flashColor);
                flash = true;
            }
            else
            {
                dollEyeMat.SetColor("_EmissionColor", initialColor);
                flash = false;
            }
            yield return new WaitForSeconds(0.2f);
        }

        dollEyeMat.SetColor("_EmissionColor", initialColor);
    }

    public void PlayerWithinRange(bool _inRange)
    {
        playerWithinRange = _inRange;

        if (!_inRange)
        {
            head.DOLocalRotate(headInitRot, 0.4f, RotateMode.Fast);
        }
    }

    public void OnShot(Revolver revolver)
    {
        DestroyDoll();
    }

    private void DestroyDoll()
    {
        // Doll death sound / Announcement?
        // Particle effect?
        // Destroy(gameObject);
    }
}
