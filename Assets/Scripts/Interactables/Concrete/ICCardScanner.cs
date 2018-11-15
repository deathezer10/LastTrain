using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ICCardScanner : MonoBehaviour
{

    [SerializeField]
    private Collider m_GantryBarrier;

    [SerializeField]
    private GameObject m_LeftGate;

    [SerializeField]
    private GameObject m_RightGate;

    const float m_DoorSwingSpeed = 0.3f;

    bool m_IsGateOpened = false;

    private AudioPlayer _audioPlayer;

    readonly private string colliderTag = "ICCard";

    private void Awake() {
        _audioPlayer = this.GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == colliderTag)
        {
            OpenGates();
        }
    }

    private void OpenGates()
    {
        if (m_IsGateOpened) return;
        m_IsGateOpened = true;

        _audioPlayer.Play("cardscanned", () =>
        {
            _audioPlayer.Play("gateopen");
            m_GantryBarrier.isTrigger = true;
            m_LeftGate.transform.DOLocalRotate(new Vector3(0, -90, 0), m_DoorSwingSpeed);
            m_RightGate.transform.DOLocalRotate(new Vector3(0, 90, 0), m_DoorSwingSpeed);

            // TODO change scanner color 
        });
    }

    public void CloseGates()
    {
        if (!m_IsGateOpened) return;
        m_IsGateOpened = false;

        _audioPlayer.Play("cardscanned", () =>
        {
            _audioPlayer.Play("gateclose");
            m_GantryBarrier.isTrigger = false;
            m_LeftGate.transform.DOLocalRotate(new Vector3(0, 0, 0), m_DoorSwingSpeed);
            m_RightGate.transform.DOLocalRotate(new Vector3(0, 0, 0), m_DoorSwingSpeed);

            // TODO change scanner color
        });
    }
}
