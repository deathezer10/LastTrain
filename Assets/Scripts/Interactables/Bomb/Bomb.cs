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

    float timeRemaining = 180;
    bool timerRunning;
    public bool isGlassBroken = false;
    TextMeshPro timerTextMesh;

    private AudioPlayer[] m_AudioPlayers;

    private AudioPlayer m_TickingAudioPlayer;

    private void Start()
    {
        timerRunning = true;
        timerTextMesh = GetComponentInChildren<TextMeshPro>();
        m_AudioPlayers = GetComponents<AudioPlayer>();
        m_TickingAudioPlayer = GetComponent<AudioPlayer>().GetLocalAudioPlayer("bomb_timer_1");
        StartCoroutine(BombCountdown());
    }

    public void StartBomb()
    {
        transform.GetComponent<Collider>().enabled = false;
        foreach (var collider in transform.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        timerRunning = true;
        timerTextMesh = GetComponentInChildren<TextMeshPro>();
        StartCoroutine(BombCountdown());
    }

    private IEnumerator BombCountdown()
    {
        int prevSeconds = Mathf.FloorToInt(timeRemaining);

        while (timeRemaining >= 0 && timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            timerTextMesh.text = string.Format("{0:0}:{1:00}", ((int)timeRemaining / 60), (int)timeRemaining % 60);

            if (prevSeconds != Mathf.FloorToInt(timeRemaining) && timeRemaining > 0)
            {
                m_TickingAudioPlayer.audioSource.pitch = Mathf.Clamp(1 + ((20 - timeRemaining) / 20), 1, 2);
                m_TickingAudioPlayer.Play();
            }

            prevSeconds = Mathf.FloorToInt(timeRemaining);

            yield return null;
        }

        if (timeRemaining < 0)
        {
            TimerTimeOut();
        }
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
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
        foreach (AudioPlayer player in m_AudioPlayers)
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
        timerTextMesh.color = green;
        timerRunning = false;

        foreach (AudioPlayer player in m_AudioPlayers)
            player.audioSource.Stop();

        StopCoroutine(BombCountdown());

        foreach (AudioPlayer player in m_AudioPlayers)
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
        AnnouncementManager.Instance.PlayAnnouncement3D("bomb_defused", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement3D("increase_speed", transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 1f);
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

        AnnouncementManager.Instance.PlayAnnouncement2D("bomb_defused", AnnouncementManager.AnnounceType.Queue, 0f);
        AnnouncementManager.Instance.PlayAnnouncement2D("increase_speed", AnnouncementManager.AnnounceType.Queue, 1f);

        FindObjectOfType<TrainBetweenCabinDoors>().OpenBetweenDoors();

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }

    public void UnlockRigidbody()
    {
        GetComponent<BoxCollider>().enabled = true;

        foreach (var collider in transform.GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;

    }

    public void OnShot(Revolver revolver)
    {
        TimerTimeOut();
    }

}