﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour , IShootable
{
    public Color[] colorArray;
    public GameObject balloonParticle;

    Vector3 upwardsMove;
    Vector3 initialPos, resetPos;

    private void Start()
    {
        upwardsMove = new Vector3(0f, Random.Range(0.3f, 0.6f), 0f);
        initialPos = transform.position;
        resetPos = initialPos;
        resetPos.y = Random.Range(-4f, -2f);
        GetComponent<MeshRenderer>().material.color = colorArray[Random.Range(0, colorArray.Length-1)];
    }

    void Update()
    {
        transform.position = transform.position + (upwardsMove * Time.deltaTime);
    }

    public void PopBalloon()
    {
        Instantiate(balloonParticle, transform.position, transform.rotation);
        transform.position = resetPos;
        upwardsMove.y = Random.Range(0.3f, 0.6f);
    }

    public void OnShot(Revolver revolver)
    {
        PopBalloon();
    }
}