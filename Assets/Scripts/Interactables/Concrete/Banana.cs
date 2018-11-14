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

    void Start()
    {
        halfRenderers = GetComponentsInChildren<MeshRenderer>();
        halfColliders = GetComponents<MeshCollider>();
        banana_ChompSound = GetComponent<AudioPlayer>();
    }

    public void BiteBanana()
    {
        if (!bittenOnce)
        {
            bittenOnce = true;

            banana_ChompSound.Play();

            halfRenderers[1].enabled = false;
            halfColliders[0].enabled = false;

            return;
        }

        banana_ChompSound.Play();

        halfRenderers[0].enabled = false;
        halfColliders[1].enabled = false;

        StartCoroutine(BananaDestroy());
    }

    private IEnumerator BananaDestroy()
    {
        yield return new WaitForSeconds(1f);

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

        transform.rotation = m_CurrentController.transform.rotation;
        transform.Rotate(new Vector3(0, -90, 0));
        transform.position = m_CurrentController.transform.position;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();
    }
}
