using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Doll : GrabbableObject, IShootable
{
    public Transform head;
    public Material dollEyeMat;
    public Color initialColor, flashColor;
    public ParticleSystem dollDeathParticle;

    AudioPlayer useAudio;

    Transform playerHeadTrans;

    Vector3 headInitRot;

    float flashEndTime;
    bool playerWithinRange;
    bool death;
    bool grabbedOnce = false;

    int burningSpots;

    DollDeathAnnouncements ddAnnouncements;

    private void Start()
    {
        playerHeadTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        dollEyeMat.SetColor("_EmissionColor", initialColor);
        headInitRot = head.localEulerAngles;
        useAudio = GetComponent<AudioPlayer>();

        ddAnnouncements = FindObjectOfType<DollDeathAnnouncements>();

        SetCollisionIgnore();
    }

    private void Update()
    {
        if (playerWithinRange)
        {
            head.LookAt(playerHeadTrans);
        }
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (grabbedOnce == false)
        {
            transform.Find("Awkward").GetComponent<AudioPlayer>().Play();
            grabbedOnce = true;
        }
    }

    private void SetCollisionIgnore()
    {
        Collider[] childrenColliders = GetComponentsInChildren<Collider>();
        CapsuleCollider headCollider = head.gameObject.GetComponent<CapsuleCollider>();

        foreach (Collider col in childrenColliders)
        {
            if (col != headCollider)
            {
                Physics.IgnoreCollision(headCollider, col);
            }
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

    public override void OnUse()
    {
        base.OnUse();

        if (!useAudio.IsPlaying())
        {
            useAudio.Play();
        }
    }

    public void OnThrownOut()
    {
        StartCoroutine(DollThrownOut());
    }

    private IEnumerator DollThrownOut()
    {
        yield return new WaitForSeconds(2f);
        DestroyDoll();
    }

    public void BurningStart()
    {
        burningSpots++;

        if (burningSpots == 2)
        {
            StartCoroutine(DollBurning());
        }
    }

    private IEnumerator DollBurning()
    {
        yield return new WaitForSeconds(2f);
        DestroyDoll();
    }

    private void DestroyDoll()
    {
        if (!death)
        {
            death = true;

            AnnouncementManager.Instance.PlayAnnouncement3D(ddAnnouncements.nextClip(), transform.position + new Vector3(0f, 10f, 0f), AnnouncementManager.AnnounceType.Queue, 0.5f);

            Instantiate(dollDeathParticle, transform.position + new Vector3(0f, 0.2f, 0), transform.rotation, null);
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Prevent invisible collider from making noise
        if (m_DropSoundHandler != null && other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            m_DropSoundHandler.PlayDropSound(GetComponent<Rigidbody>().velocity);
            Debug.Log("dog: " + other.name);
        }
    }

}
