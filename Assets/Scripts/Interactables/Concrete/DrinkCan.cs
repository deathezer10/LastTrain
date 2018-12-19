using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrinkCan : GrabbableObject
{
    [SerializeField]
    Transform m_OpenerTransform;

    [SerializeField]
    MeshRenderer m_CoverRenderer;

    [SerializeField]
    Vector3 m_OpenerEndRot;

    bool m_bOpened;

    private void Start()
    {
        GetComponent<ParticleSystem>().Stop();
    }

    public override void OnGrab()
    {
        base.OnGrab();
    }

    public override void OnUse()
    {
        base.OnUse();

        if (!m_bOpened)
        {
            m_bOpened = true;

            m_OpenerTransform.DOLocalRotate(m_OpenerEndRot, 0.1f, RotateMode.Fast);
            m_CoverRenderer.enabled = false;

            GetComponent<AudioPlayer>().Play();

            GetComponent<ParticleSystem>().Play();

            GetComponentInChildren<AudioPlayer>().Play();
        }
    }
}
