using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGrenade : GrabbableObject, IShootable
{
    public GameObject grenadeParticlePrefab;
    public GameObject pinVisual, ringVisual, pinPhysics, ringPhysics;

    AudioPlayer[] grenadeSounds;

    PlayerViveController m_CurrentController;

    bool bUsedOnce;

    private void Start()
    {
        grenadeSounds = GetComponents<AudioPlayer>();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Metal });
    }

    private void Explode()
    {
        base.OnGrabReleased();

        if (m_CurrentController != null)
            m_CurrentController.ToggleControllerModel(true);

        Instantiate(grenadeParticlePrefab, transform.position, grenadeParticlePrefab.transform.rotation, null);
        Destroy(gameObject);
    }

    private void PullPin()
    {
        grenadeSounds[0].Play();

        pinVisual.SetActive(false);
        ringVisual.SetActive(false);

        pinPhysics.SetActive(true);
        pinPhysics.transform.SetParent(null);

        ringPhysics.SetActive(true);
        ringPhysics.transform.SetParent(null);

        StartCoroutine(PinPullTimer());
    }

    private IEnumerator PinPullTimer()
    {
        yield return new WaitForSeconds(0.15f);
        grenadeSounds[1].Play();
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().volume = 0.3f;
        yield return new WaitForSeconds(3.15f);
        Explode();
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
            transform.Rotate(new Vector3(0, 0, 0f));
            transform.position = m_CurrentController.transform.position;
        }
    }

    public override void OnUse()
    {
        if (!bUsedOnce)
        {
            bUsedOnce = true;
            PullPin();
        }
    }

    public void OnShot(Revolver revolver)
    {
        Explode();
    }
}
