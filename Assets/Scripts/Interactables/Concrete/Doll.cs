using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Doll : GrabbableObject, IShootable
{
    public Transform head, headTarget;
    public Material dollEyeMat;
    public Color initialColor, flashColor, bombHintColor;
    public ParticleSystem dollDeathParticle;
    public int dollIndex;
    public GameObject dollSmoke;

    AudioPlayer useAudio;

    Transform playerHeadTrans;

    Vector3 headInitRot;

    float flashEndTime;
    bool playerWithinRange;
    bool death;
    bool grabbedOnce = false;
    bool showingHint;
    bool burnStarted;

    DollDeathAnnouncements ddAnnouncements;

    Vector3 targetRot = new Vector3();

    private void Start()
    {
        playerHeadTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        dollEyeMat.SetColor("_EmissionColor", initialColor);
        headInitRot = head.localEulerAngles;
        useAudio = GetComponent<AudioPlayer>();

        dollSmoke.SetActive(false);

        ddAnnouncements = FindObjectOfType<DollDeathAnnouncements>();

        SetCollisionIgnore();
    }

    private void Update()
    {
        if (playerWithinRange)
        {
            headTarget.eulerAngles = Vector3.zero;
            headTarget.LookAt(playerHeadTrans);

            if (!(headTarget.localEulerAngles.x > 20f && headTarget.localEulerAngles.x < 325f))
            {
                targetRot.x = headTarget.localEulerAngles.x;
            }

            if (!(headTarget.localEulerAngles.y > 45f && headTarget.localEulerAngles.y < 315f))
            {
                targetRot.y = headTarget.localEulerAngles.y;
            }
            targetRot.z = 0;

            if (head.eulerAngles != targetRot)
            {
                head.DOLocalRotate(targetRot, Time.deltaTime * 3, RotateMode.Fast);
            }
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
        dollEyeMat.SetColor("_EmissionColor", initialColor);

        flashEndTime = Time.time + _duration;
        StopAllCoroutines();
        StartCoroutine(EyeFlash());
    }

    private void StartBlueFlash()
    {
        showingHint = true;

        dollEyeMat.SetColor("_EmissionColor", initialColor);

        StopAllCoroutines();
        StartCoroutine(BlueFlash());
    }

    private IEnumerator BlueFlash()
    {
        dollEyeMat.SetColor("_EmissionColor", bombHintColor);
        yield return new WaitForSeconds(1.5f);
        dollEyeMat.SetColor("_EmissionColor", initialColor);
        showingHint = false;
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

        if (!showingHint)
        {
            useAudio.Play();

            if (dollIndex == 1)
            {
                StartBlueFlash();
            }
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
        if (burnStarted)
        {
            return;
        }

        burnStarted = true;

        dollSmoke.SetActive(true);
        StartCoroutine(DollBurning());
    }

    private IEnumerator DollBurning()
    {
        yield return new WaitForSeconds(10f);
        DestroyDoll();
    }

    private void DestroyDoll()
    {
        if (!death)
        {
            death = true;

            AnnouncementManager.Instance.PlayAnnouncement3D(ddAnnouncements.nextClip(), AnnouncementManager.AnnounceType.Queue, 0.5f);

            Instantiate(dollDeathParticle, transform.position, transform.rotation, null);
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Prevent invisible collider from making noise
        if (m_DropSoundHandler != null && other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            m_DropSoundHandler.PlayDropSound(GetComponent<Rigidbody>().velocity);
        }
    }

}
