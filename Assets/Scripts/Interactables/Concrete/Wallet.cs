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

    private BoxCollider[] m_Colliders;

    [SerializeField]
    private BoxCollider[] m_OpeningColliders;

    private void Start()
    {
        m_TManager = FindObjectOfType<TutorialManager>();
        m_TManagerAudioPlayer = m_TManager.GetComponent<AudioPlayer>();
        m_Colliders = GetComponents<BoxCollider>();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Plastic });

        Physics.IgnoreCollision(m_OpeningColliders[0], m_OpeningColliders[1]);
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);
        m_Controller = currentController;
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (m_Controller == null)
            return;

        //transform.position = m_Controller.transform.position;
        //transform.rotation = m_Controller.transform.rotation;

        if (m_TutorialArrow != null && m_TutorialArrow.activeInHierarchy)
            m_TutorialArrow.SetActive(false);

        if (!m_HasAnnounced)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.Poster3);

            FindObjectOfType<AnnouncementManager>().PlayAnnouncement3D("wallet_pickup", transform.position + new Vector3(0f, 5f, 0f), AnnouncementManager.AnnounceType.Queue, 0f);

            //m_TManagerAudioPlayer.Play("newtutorial_trainarriving", () => { m_TManagerAudioPlayer.Play("newtutorial_trainarriving"); }, 2);
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

            m_Colliders[0].center = m_Colliders[1].center;
            m_Colliders[0].size = m_Colliders[1].size;

            GameObject obj = Instantiate(m_ICCardPrefab, transform.position, Quaternion.identity);
            Physics.IgnoreCollision(m_OpeningColliders[0], obj.GetComponent<Collider>());
            Physics.IgnoreCollision(m_OpeningColliders[1], obj.GetComponent<Collider>());

            m_HasUsedOnce = true;
        }
    }

    public void OnWalletOpened()
    {
    }

}