using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyGrenade : GrabbableObject, IShootable
{
    GameObject grenadeParticlePrefab;

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

    public override void OnGrab()
    {
        base.OnGrab();
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
