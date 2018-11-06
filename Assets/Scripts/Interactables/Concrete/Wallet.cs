using System.Collections;
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
        if (!m_HasAnnounced)
        {
            m_TManager.m_ImageWallet.MoveToHolder();
            m_TManager.m_ImageGripButton.gameObject.SetActive(true);
            m_TManagerAudioPlayer.Stop();
            m_TManagerAudioPlayer.Play("tutorial_wallet_outro");
            m_HasAnnounced = true;
        }
    }

    public override void OnUse()
    {
        if (!m_HasUsedOnce)
        {
            m_TManager.m_ImageGripButton.MoveToHolder();
            m_TManager.m_ImageUnlockGates.gameObject.SetActive(true);
            m_TManagerAudioPlayer.Stop();
            m_TManagerAudioPlayer.Play("tutorial_ic_card");

            GetComponent<Animator>().Play("Open");
            m_colliders[1].enabled = true;
            m_colliders[0].enabled = false;

            GameObject obj = Instantiate(m_ICCardPrefab,transform.position, Quaternion.identity);
            Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
            m_HasUsedOnce = true;
        }
    }

}
