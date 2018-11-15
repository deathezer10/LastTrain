using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGrenade : GrabbableObject, IShootable
{
    public GameObject grenadeParticlePrefab;
    public GameObject pinVisual, ringVisual, pinPhysics, ringPhysics;

    PlayerViveController m_CurrentController;

    private void Explode()
    {
        base.OnGrabReleased();
        m_CurrentController.ToggleControllerModel(true);
        Instantiate(grenadeParticlePrefab, transform.position, transform.rotation, null);
        Destroy(gameObject);
    }

    private void PullPin()
    {
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
        yield return new WaitForSeconds(2.5f);
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

        transform.rotation = m_CurrentController.transform.rotation;
        transform.Rotate(new Vector3(0, 0, -15f));
        transform.position = m_CurrentController.transform.position;
    }

    public override void OnUse()
    {
        PullPin();
    }

    public void OnShot(Revolver revolver)
    {
        Explode();
    }
}
