using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class TutorialManager : MonoBehaviour
{

    [SerializeField]
    private bool m_TutorialEnabled = true;
    public bool tutorialEnabled {
        get { return m_TutorialEnabled; }
        set { m_TutorialEnabled = value; }
    }

    [SerializeField]
    private SteamVR_Action_Boolean _padAction;
    
    public InstructionImage m_ImageTeleport, m_ImageWallet, m_ImageGripButton, m_ImageUnlockGates;

    private PlayerTeleportation m_PlayerTeleportation;
    
    void Start()
    {
        tutorialEnabled = Checkpoint.CHECKPOINT_ACTIVATED ? false : true;

        if (tutorialEnabled)
        {
            m_PlayerTeleportation = FindObjectOfType<PlayerTeleportation>();

            if (m_PlayerTeleportation != null)
                m_PlayerTeleportation.gameObject.SetActive(false);

            m_ImageTeleport = Instantiate(m_ImageTeleport);
            m_ImageTeleport.gameObject.SetActive(false);

            m_ImageWallet = Instantiate(m_ImageWallet);
            m_ImageWallet.gameObject.SetActive(false);

            m_ImageGripButton = Instantiate(m_ImageGripButton);
            m_ImageGripButton.gameObject.SetActive(false);

            m_ImageUnlockGates = Instantiate(m_ImageUnlockGates);
            m_ImageUnlockGates.gameObject.SetActive(false);

            Invoke("StartTutorial", 1.5f);
        }
    }

    void StartTutorial()
    {

        var audioPlayer = GetComponent<AudioPlayer>();

        audioPlayer.Play("tutorial_greeting", () =>
        {
            audioPlayer.Play("tutorial_trajectory_intro");

            if (m_PlayerTeleportation != null)
                m_PlayerTeleportation.gameObject.SetActive(true);

            System.IDisposable padObserver = null;

            m_ImageTeleport.gameObject.SetActive(true);

            // Teleport intro
            padObserver = this.UpdateAsObservable()
           .Where(_ => _padAction.GetStateDown(SteamVR_Input_Sources.Any)) // Input.GetKeyDown(KeyCode.A))
           .Subscribe(_ =>
           {
               padObserver.Dispose();
               audioPlayer.Stop();
               audioPlayer.Play("tutorial_trajectory_outro");

               // Teleport outro
               padObserver = this.UpdateAsObservable()
               .Where(_1 => _padAction.GetStateUp(SteamVR_Input_Sources.Any)) // Input.GetKeyUp(KeyCode.A))
               .Subscribe(_1 =>
               {
                   m_ImageTeleport.MoveToHolder();

                   padObserver.Dispose();
                   audioPlayer.Stop();
                   audioPlayer.Play("tutorial_wallet_intro");

                   m_ImageWallet.gameObject.SetActive(true);

                   // Wallet Intro will be in the Wallet.cs script
               });
           }
           );

        }, 0.5f);

    }

}
