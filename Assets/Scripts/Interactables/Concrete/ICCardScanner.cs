using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ICCardScanner : MonoBehaviour
{

    [SerializeField]
    private GameObject m_GantryBarrier;

    [SerializeField]
    private GameObject m_LeftGate;

    [SerializeField]
    private GameObject m_RightGate;

    const float m_DoorSwingSpeed = 0.3f;

    bool m_IsGateOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        ICCard wallet = other.GetComponent<ICCard>();

        if (wallet != null)
        {
            OpenGates();
        }
    }

    public void OpenGates()
    {
        if (!m_IsGateOpened)
        {
            m_IsGateOpened = true;

            var audioPlayer = GetComponent<AudioPlayer>();
            audioPlayer.Play("cardscanned", () =>
            {
                audioPlayer.Play("gateopen");
                m_GantryBarrier.GetComponent<Collider>().isTrigger = true;
                m_LeftGate.transform.DOLocalRotate(new Vector3(0, -90, 0), m_DoorSwingSpeed);
                m_RightGate.transform.DOLocalRotate(new Vector3(0, 90, 0), m_DoorSwingSpeed);

                // TODO change scanner color 

            });
        }
    }

    public void CloseGates()
    {
        if (m_IsGateOpened)
        {
            m_IsGateOpened = false;

            var audioPlayer = GetComponent<AudioPlayer>();
            audioPlayer.Play("cardscanned", () =>
            {
                audioPlayer.Play("gateclose");
                m_GantryBarrier.GetComponent<Collider>().isTrigger = false;
                m_LeftGate.transform.DOLocalRotate(new Vector3(0, 0, 0), m_DoorSwingSpeed);
                m_RightGate.transform.DOLocalRotate(new Vector3(0, 0, 0), m_DoorSwingSpeed);

                // TODO change scanner color

            });
        }
    }

}
