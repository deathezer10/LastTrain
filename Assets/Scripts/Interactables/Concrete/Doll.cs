using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Script for controlling a Plushie Doll Prefab object in the scene. Can be picked up, set on fire and shot.
/// Dolls eyes flash when an announcement is being made, plays a sound effect when Used, 
/// gives a slight hint to a puzzle if the doll is the correct one (index)
/// </summary>
public class Doll : GrabbableObject, IShootable
{
    #region References to be set in Inspector

    [SerializeField]
    Transform m_DollHead, m_DollHeadTarget;

    [SerializeField]
    Material m_DollEyeMat;

    [SerializeField]
    Color m_InitialColor, m_FlashColor, m_BombHintColor;

    [SerializeField]
    ParticleSystem m_DollDeathParticle;

    [SerializeField]
    int m_DollIndex;

    [SerializeField]
    GameObject m_DollSmoke;

    #endregion

    AudioPlayer m_UseAudio;

    Transform m_PlayerHeadTrans;

    Vector3 m_HeadInitRot;

    Tweener m_DollHeadTweener;

    float m_FlashEndTime;

    bool bPlayerWithinRange, bDying, bGrabbedOnce, bShowingHint, bBurnStarted;

    // Script listing the possible clip String aliases used for calling Announcement manager
    DollDeathAnnouncements m_DDAnnouncements;

    Vector3 m_TargetRot = new Vector3();

    private void Start()
    {
        m_PlayerHeadTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        m_UseAudio = GetComponent<AudioPlayer>();
        m_DDAnnouncements = FindObjectOfType<DollDeathAnnouncements>();

        m_DollSmoke.SetActive(false);
        m_DollEyeMat.SetColor("_EmissionColor", m_InitialColor);
        m_HeadInitRot = m_DollHead.localEulerAngles;

        SetCollisionIgnore();
    }

    private void Update()
    {
        // If the Doll can "See" the Player's head, it will turn to face and look at the Player
        if (bPlayerWithinRange)
        {
            m_DollHeadTarget.eulerAngles = Vector3.zero;
            m_DollHeadTarget.LookAt(m_PlayerHeadTrans);

            if (!(m_DollHeadTarget.localEulerAngles.x > 20f && m_DollHeadTarget.localEulerAngles.x < 325f))
            {
                m_TargetRot.x = m_DollHeadTarget.localEulerAngles.x;
            }

            if (!(m_DollHeadTarget.localEulerAngles.y > 45f && m_DollHeadTarget.localEulerAngles.y < 315f))
            {
                m_TargetRot.y = m_DollHeadTarget.localEulerAngles.y;
            }

            m_TargetRot.z = 0;

            if (m_DollHead.eulerAngles != m_TargetRot)
            {
                // Using Unity Tweener Extension to tween Doll's head rotation towards the player's head
                if (m_DollHeadTweener != null && !m_DollHeadTweener.IsPlaying())
                {
                    m_DollHeadTweener = m_DollHead.DOLocalRotate(m_TargetRot, Time.deltaTime * 6, RotateMode.Fast);
                }
            }
        }
    }

    // Implementation of the Inherited OnGrab from GrabbableObject, used for Vive Controller Grabbing actions
    public override void OnGrab()
    {
        base.OnGrab();

        if (!bGrabbedOnce)
        {
            transform.Find("Awkward").GetComponent<AudioPlayer>().Play();
            bGrabbedOnce = true;
        }
    }

    /// <summary>
    /// Stop the Dolls parts from colliding with each other
    /// </summary>
    private void SetCollisionIgnore()
    {
        Collider[] childrenColliders = GetComponentsInChildren<Collider>();
        CapsuleCollider headCollider = m_DollHead.gameObject.GetComponent<CapsuleCollider>();

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
    public void StartEyeFlash(float duration)
    {
        m_DollEyeMat.SetColor("_EmissionColor", m_InitialColor);

        m_FlashEndTime = Time.time + duration;
        StopAllCoroutines();
        StartCoroutine(EyeFlash());
    }

    // Flash the Doll's eyes blue, as a hint for a puzzle on the train
    private void StartBlueFlash()
    {
        bShowingHint = true;

        m_DollEyeMat.SetColor("_EmissionColor", m_InitialColor);

        StopAllCoroutines();
        StartCoroutine(BlueFlash());
    }

    private IEnumerator BlueFlash()
    {
        m_DollEyeMat.SetColor("_EmissionColor", m_BombHintColor);
        yield return new WaitForSeconds(1.5f);
        m_DollEyeMat.SetColor("_EmissionColor", m_InitialColor);
        bShowingHint = false;
    }

    // Flash the dolls eyes while an announcement is playing, alternating between two colors
    private IEnumerator EyeFlash()
    {
        bool flash = false;

        while (Time.time <= m_FlashEndTime)
        {
            if (!flash)
            {
                m_DollEyeMat.SetColor("_EmissionColor", m_FlashColor);
                flash = true;
            }
            else
            {
                m_DollEyeMat.SetColor("_EmissionColor", m_InitialColor);
                flash = false;
            }
            yield return new WaitForSeconds(0.2f);
        }

        m_DollEyeMat.SetColor("_EmissionColor", m_InitialColor);
    }

    /// <summary>
    /// Called from a child objects OnColliderEnter to set whether or not the Player is in range of the Doll
    /// </summary>
    public void PlayerWithinRange(bool _inRange)
    {
        bPlayerWithinRange = _inRange;

        if (!_inRange)
        {
            // Rotate Doll's head back to original when Player leaves line of sight
            m_DollHead.DOLocalRotate(m_HeadInitRot, 0.4f, RotateMode.Fast);
        }
    }

    // Implementation of IShootable Interface
    public void OnShot(Revolver revolver)
    {
        DestroyDoll();
    }

    // Implementation of the Inherited OnUse from GrabbableObject, used for Vive Controller Use actions
    public override void OnUse()
    {
        base.OnUse();

        if (!bShowingHint)
        {
            m_UseAudio.Play();

            if (m_DollIndex == 1)
            {
                StartBlueFlash();
            }
        }
    }

    /// <summary>
    /// Called when the Doll is thrown out of the Train and should be destroyed
    /// </summary>
    public void OnThrownOut()
    {
        StartCoroutine(DollThrownOut());
    }

    private IEnumerator DollThrownOut()
    {
        yield return new WaitForSeconds(2f);
        DestroyDoll();
    }

    /// <summary>
    /// Called from a child objects script, through an interaction with other objects. Sets the doll on fire.
    /// </summary>
    public void BurningStart()
    {
        if (bBurnStarted)
        {
            return;
        }

        bBurnStarted = true;

        m_DollSmoke.SetActive(true);
        StartCoroutine(DollBurning());
    }

    private IEnumerator DollBurning()
    {
        yield return new WaitForSeconds(10f);
        DestroyDoll();
    }

    private void DestroyDoll()
    {
        if (!bDying)
        {
            bDying = true;

            // Call the Announcement Manager to play an announcement, based on a clip alias from DollDeathAnnouncements
            AnnouncementManager.Instance.PlayAnnouncement3D(m_DDAnnouncements.nextClip(), AnnouncementManager.AnnounceType.Queue, 0.5f);

            Instantiate(m_DollDeathParticle, transform.position, transform.rotation, null);
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // Prevent invisible collider from making noise with DropSoundHandler
        if (m_DropSoundHandler != null && other.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            m_DropSoundHandler.PlayDropSound(GetComponent<Rigidbody>().velocity);
        }
    }

}
