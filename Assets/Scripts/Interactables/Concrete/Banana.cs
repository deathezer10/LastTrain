using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : GrabbableObject
{
    MeshRenderer[] halfRenderers;

    MeshCollider[] halfColliders;

    AudioPlayer banana_ChompSound;

    PlayerViveController m_CurrentController;

    bool bittenOnce;

    bool isGrabbed = false;

    void Start()
    {
        halfRenderers = GetComponentsInChildren<MeshRenderer>();
        halfColliders = GetComponentsInChildren<MeshCollider>();
        banana_ChompSound = GetComponent<AudioPlayer>();
    }

    public void BiteBanana()
    {
        if (isGrabbed == false)
            return;

        if (!bittenOnce)
        {
            bittenOnce = true;

            banana_ChompSound.Play();

            halfRenderers[1].enabled = false;
            halfColliders[1].enabled = false;

            return;
        }

        banana_ChompSound.Play();

        halfRenderers[0].enabled = false;
        halfColliders[0].enabled = false;

        m_CurrentController.ToggleControllerModel(true);
        StartCoroutine(BananaDestroy());
    }

    private IEnumerator BananaDestroy()
    {
        yield return new WaitForSeconds(0.55f);

        OnGrabReleased();
        Destroy(gameObject);
    }

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);
        m_CurrentController = currentController;
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (m_CurrentController != null)
        {
            transform.rotation = m_CurrentController.transform.rotation;
            transform.Rotate(new Vector3(0, 90, -50));
            transform.position = m_CurrentController.transform.position;
        }

        isGrabbed = true;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();
        isGrabbed = false;
    }

}
