using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : GrabbableObject
{

    [SerializeField]
    private GameObject m_ICCardPrefab;

    [SerializeField]
    private GameObject m_TutorialArrow;

    [SerializeField]
    private Transform m_ICSpawnSpot;

    private bool m_HasAnnounced = false;
    private bool m_HasUsedOnce = false;

    TutorialManager m_TManager;

    PlayerViveController m_Controller;

    private BoxCollider[] m_Colliders;

    [SerializeField]
    private BoxCollider[] m_OpeningColliders;

    private Animator m_animator;

    private void Start()
    {
        m_TManager = FindObjectOfType<TutorialManager>();
        m_Colliders = GetComponents<BoxCollider>();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Plastic });

        Physics.IgnoreCollision(m_OpeningColliders[0], m_OpeningColliders[1]);

        m_animator = this.GetComponent<Animator>();
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
        {
            return;
        }
            
        if (m_TutorialArrow != null && m_TutorialArrow.activeInHierarchy)
            m_TutorialArrow.SetActive(false);

        if (!m_HasAnnounced)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.Poster3);

            AnnouncementManager.Instance.PlayAnnouncement3D("announcement_chime", AnnouncementManager.AnnounceType.Queue, 0f);
            AnnouncementManager.Instance.PlayAnnouncement3D("wallet_pickup", AnnouncementManager.AnnounceType.Queue, 0f);
            
            m_HasAnnounced = true;
        }
    }

    public override void OnUse()
    {
        base.OnUse();

        if (!m_HasUsedOnce)
        {
            m_TManager.SetPoster(TutorialManager.PosterState.None);

            m_animator.Play("Open");

            m_Colliders[0].center = m_Colliders[1].center;
            m_Colliders[0].size = m_Colliders[1].size;

            // GameObject obj = 
            //var collider = obj.GetComponent<Collider>();
            //Physics.IgnoreCollision(m_OpeningColliders[0], collider);
            //Physics.IgnoreCollision(m_OpeningColliders[1], collider);

            m_HasUsedOnce = true;
        }
    }

    public void OnWalletOpened()
    {
        Instantiate(m_ICCardPrefab, m_ICSpawnSpot.position, Quaternion.identity);
    }
}