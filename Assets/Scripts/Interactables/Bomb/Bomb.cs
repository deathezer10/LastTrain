using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : GrabbableObject, IShootable
{
    public MeshRenderer phLightRenderer;
    public Color red, green;
    public GameObject explosionParticlePrefab;

    Vector3 explosionOffset1 = new Vector3(0f, -0.2f, 0.4f);
    Vector3 explosionOffset2 = new Vector3(0f, 0.3f, -0.5f);

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
            if (player.clip.name == "bomb_timer_1")
            {
                player.Play();
                break;
            }
        }
    }

    private IEnumerator BombCountdown()
    {
        while (timeRemaining >= 0 && timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            timerTextMesh.text = string.Format("{0:0}:{1:00}", ((int)timeRemaining / 60), (int)timeRemaining % 60);

            if (timeRemaining <= 20 && timeRemaining > 0)
            {
                foreach (AudioPlayer player in audioPlayers)
                {
                    if (player.clip.name == "bomb_timer_1")
                    {
                        player.audioSource.pitch = 1 + ((20 - timeRemaining) / 20);
                        break;
                    }
                }
            }

            yield return null;
        }

        if (timeRemaining < 0)
        {
            TimerTimeOut();
        }
    }
    
    public override bool hideControllerOnGrab { get { return true; } }

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

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

    public void TimerTimeOut()
    {
        phLightRenderer.material.color = red;
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_bomb");
        InstantiateExplosions();
        PlayExplosionSound();
    }

    public void CutWrongWire()
    {
        phLightRenderer.material.color = red;
        FindObjectOfType<PlayerDeathHandler>().KillPlayer("death_bombwrongwire");
        InstantiateExplosions();
        PlayExplosionSound();
    }

    private void PlayExplosionSound()
    {
        foreach (AudioPlayer player in audioPlayers)
        {
            if (player.clip.name == "bomb_explosion_1")
            {
                player.audioSource.volume = 1;
                player.audioSource.loop = false;
                player.Play();
                break;
            }
        }
    }

    private void InstantiateExplosions()
    {
        Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
        Instantiate(explosionParticlePrefab, transform.position + explosionOffset1, transform.rotation);
        Instantiate(explosionParticlePrefab, transform.position + explosionOffset2, transform.rotation);
    }

    public void CutRightWire()
    {
        phLightRenderer.material.color = green;
        timerRunning = false;

        foreach (AudioPlayer player in audioPlayers)
            player.audioSource.Stop();

        StopCoroutine(BombCountdown());

        foreach (AudioPlayer player in audioPlayers)
        {
            if (player.clip.name == "bomb_defused")
            {
                player.audioSource.volume = 1f;
                player.audioSource.loop = false;
                player.Play();
                break;
            }
        }

        FindObjectOfType<TrainBetweenCabinDoors>().OpenBetweenDoors();
    }

    public void ThrownOut()
    {
        timerRunning = false;
        StopCoroutine(BombCountdown());

        StartCoroutine(ThrownOutDelay());
    }

    IEnumerator ThrownOutDelay()
    {
        yield return new WaitForSeconds(2f);

        FindObjectOfType<TrainBetweenCabinDoors>().OpenBetweenDoors();

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    public void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }

    public void OnShot(Revolver revolver)
    {
        TimerTimeOut();
    }

}