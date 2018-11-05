using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : GrabbableObject
{

    [SerializeField]
    private GameObject m_ICCardPrefab;

    private bool m_HasAnnounced = false;
    private bool m_HasUsedOnce = false;

    TutorialManager m_TManager;
    AudioPlayer m_TManagerAudioPlayer;

    private void Start()
    {
        m_TManager = FindObjectOfType<TutorialManager>();
        m_TManagerAudioPlayer = m_TManager.GetComponent<AudioPlayer>();
    }

    public override void OnGrab()
    {
        if (!m_HasAnnounced)
        {
            m_TManagerAudioPlayer.Stop();
            m_TManagerAudioPlayer.Play("tutorial_wallet_outro");
            m_HasAnnounced = true;
        }
    }

    public override void OnUse()
    {
        if (!m_HasUsedOnce)
        {
            m_TManagerAudioPlayer.Stop();
            m_TManagerAudioPlayer.Play("tutorial_ic_card");
            Instantiate(m_ICCardPrefab);
            m_HasUsedOnce = true;
        }
    }

}
