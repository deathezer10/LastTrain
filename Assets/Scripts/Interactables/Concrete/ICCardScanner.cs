using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ICCardScanner : MonoBehaviour
{

    [SerializeField]
    GameObject m_GantryBarrier;

    [SerializeField]
    GameObject m_LeftGate, m_RightGate;

    bool m_ScannerBypassed = false;

    private void OnTriggerEnter(Collider other)
    {
        ICCard wallet = other.GetComponent<ICCard>();

        if (wallet != null && m_ScannerBypassed == false)
        {
            m_ScannerBypassed = true;
            OpenGates();
        }
    }

    public void OpenGates()
    {
        var tManager = FindObjectOfType<TutorialManager>();
        //tManager.GetComponent<AudioPlayer>().Play("tutorial_finale");
        //tManager.m_ImageUnlockGates.MoveToHolder();
        var audioPlayer = GetComponent<AudioPlayer>();
        audioPlayer.Play("cardscanned", () =>
        {
            audioPlayer.Play("gateopen");
            m_GantryBarrier.SetActive(false);
            m_LeftGate.transform.DOLocalRotate(new Vector3(0, -90, 0), 0.3f);
            m_RightGate.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.3f);

            // TODO change scanner color and rotate the gates

        });
    }

}
