using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : GrabbableObject
{
    public MeshRenderer phLightRenderer;
    public Color red, green;

    float timeRemaining = 180f;
    bool timerRunning;
    TextMeshPro timerTextMesh;

    private AudioPlayer[] audioPlayers;

    private void Start()
    {
        timerRunning = true;
        timerTextMesh = GetComponentInChildren<TextMeshPro>();
        StartCoroutine(BombCountdown());
        audioPlayers = GetComponents<AudioPlayer>();

        foreach (AudioPlayer player in audioPlayers)
        {
            if (player.clip.name == "bomb_timer_loop")
            {
                player.Play();
                break;
            }
        }
    }

    private IEnumerator BombCountdown()
    {
        if (timeRemaining <= 20 && timeRemaining > 0)
        {
            foreach (AudioPlayer player in audioPlayers)
            {
                if (player.clip.name == "bomb_timer_loop")
                {
                    player.audioSource.pitch = 1.5f;
                    break;
                }
            }
        }

        while (timeRemaining >= 0 && timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            timerTextMesh.text = string.Format("{0:0}:{1:00}", ((int)timeRemaining / 60), (int)timeRemaining % 60);

            yield return null;
        }

        if (timeRemaining < 0)
        {
            TimerTimeOut();
        }
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
    }

    public override void OnControllerExit()
    {
    }

    public override void OnControllerStay()
    {
    }

    public override void OnGrab()
    {
    }

    public override void OnGrabReleased()
    {
    }

    public override void OnUse()
    {
    }

    private void TimerTimeOut()
    {
        phLightRenderer.material.color = red;
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_bomb");

        foreach (AudioPlayer player in audioPlayers)
        {
            if (player.clip.name == "bomb_explosion_1")
            {
                player.audioSource.loop = false;
                player.Play();
                break;
            }
        }
    }

    public void CutWrongWire()
    {
        phLightRenderer.material.color = red;
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_bombwrongwire");
    }

    public void CutRightWire()
    {
        phLightRenderer.material.color = green;
        timerRunning = false;
        StopCoroutine(BombCountdown());
    }

    public void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }
}