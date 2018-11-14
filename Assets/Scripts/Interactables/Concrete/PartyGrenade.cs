using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGrenade : GrabbableObject, IShootable
{
    GameObject grenadeParticlePrefab;

    PlayerViveController m_CurrentController;

    private void Explode()
    {
        // Boom?
    }

    private void PullPin()
    {
        StartCoroutine(PinPullTimer());
    }

    private IEnumerator PinPullTimer()
    {
        yield return new WaitForSeconds(2f);
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
        transform.Rotate(new Vector3(0, -90, 0));
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
