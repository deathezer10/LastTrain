﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : GrabbableObject
{

    [SerializeField]
    private GameObject m_ICCardPrefab;

    private bool m_HasAnnounced = false;
    private bool m_HasUsedOnce = false;

    BoxCollider[] m_colliders;
    TutorialManager m_TManager;
    AudioPlayer m_TManagerAudioPlayer;

    private void Start()
    {
        m_TManager = FindObjectOfType<TutorialManager>();
        m_TManagerAudioPlayer = m_TManager.GetComponent<AudioPlayer>();
        m_colliders = GetComponents<BoxCollider>();
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (!m_HasAnnounced)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.Poster3);

            m_TManagerAudioPlayer.Play("newtutorial_trainarriving", () => { m_TManagerAudioPlayer.Play("newtutorial_trainarriving"); }, 2);
            m_HasAnnounced = true;
        }
    }

    public override void OnUse()
    {
        if (!m_HasUsedOnce)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.None);

            GetComponent<Animator>().Play("Open");
            m_colliders[1].enabled = true;
            m_colliders[0].enabled = false;

            GameObject obj = Instantiate(m_ICCardPrefab, transform.position + new Vector3(0f, 0f, 0.15f), Quaternion.identity);

            foreach (Collider col in GetComponents<Collider>())
            {
                Physics.IgnoreCollision(col, obj.GetComponent<Collider>());
            }

            m_HasUsedOnce = true;
        }
    }

}
