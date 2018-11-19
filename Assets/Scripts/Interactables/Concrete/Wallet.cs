using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : GrabbableObject
{

    [SerializeField]
    private GameObject m_ICCardPrefab;

    [SerializeField]
    private GameObject m_TutorialArrow;

    private bool m_HasAnnounced = false;
    private bool m_HasUsedOnce = false;

    TutorialManager m_TManager;
    AudioPlayer m_TManagerAudioPlayer;

    PlayerViveController m_Controller;

    private Collider[] m_Colliders;

    private void Start()
    {
        m_TManager = FindObjectOfType<TutorialManager>();
        m_TManagerAudioPlayer = m_TManager.GetComponent<AudioPlayer>();
        m_Colliders = GetComponents<Collider>();
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);
        m_Controller = currentController;
    }

    public override void OnGrab()
    {
        base.OnGrab();

        transform.position = m_Controller.transform.position;
        transform.rotation = m_Controller.transform.rotation;

        if (m_TutorialArrow != null && m_TutorialArrow.activeInHierarchy)
            m_TutorialArrow.SetActive(false);

        if (!m_HasAnnounced)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.Poster3);

            m_TManagerAudioPlayer.Play("newtutorial_trainarriving", () => { m_TManagerAudioPlayer.Play("newtutorial_trainarriving"); }, 2);
            m_HasAnnounced = true;
        }
    }

    public override void OnUse()
    {
        base.OnUse();

        if (!m_HasUsedOnce)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.None);

            GetComponent<Animator>().Play("Open");
            
            m_Colliders[0].bounds.SetMinMax(m_Colliders[1].bounds.min, m_Colliders[1].bounds.max);

            GameObject obj = Instantiate(m_ICCardPrefab, transform.position + new Vector3(0f, 0f, 0.15f), Quaternion.identity);
            Physics.IgnoreCollision(m_Colliders[0], obj.GetComponent<Collider>());

            m_HasUsedOnce = true;
        }
    }

    public void OnWalletOpened()
    {
    }

}