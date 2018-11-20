using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLock : GrabbableObject, IShootable
{
    public GameObject uncutTopPart, cutTopPart, umbrella;

    private void Start()
    {
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Metal });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SawBlade" && other.GetComponentInParent<SawBlade>().IsSpinning())
        {
            BreakLock();
        }
    }

    private void BreakLock()
    {
        BreakPieces();
        UnlockRigidbody();
        GetComponent<CapsuleCollider>().enabled = false;
        FindObjectOfType<UmbrCompartment>().OpenCompartment();
        umbrella.SetActive(true);
        umbrella.transform.SetParent(null);
    }

    private void BreakPieces()
    {
        uncutTopPart.SetActive(false);
        cutTopPart.SetActive(true);
        cutTopPart.transform.SetParent(null);
    }

    private void UnlockRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
    }

    public void OnShot(Revolver revolver)
    {
        BreakLock();
    }
}