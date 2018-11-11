using System;
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

    [SerializeField]
    private GameObject m_TutorialPoster1, m_TutorialPoster2;

    private Vector3 m_playerCheckpointPos = new Vector3(1f, 1.5f, -5f);
    private Vector3 m_playerCheckpointRot = new Vector3(0, -90f, 0);


    void Start()
    {
        tutorialEnabled = Checkpoint.CHECKPOINT_ACTIVATED ? false : true;

        if (tutorialEnabled)
        {
            StartTutorial();
        }
        else  // Skipping tutorial to checkpoint position * or just move this check to a seperate check script on the player object
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.transform.position = m_playerCheckpointPos;
            player.transform.eulerAngles = m_playerCheckpointRot;
        }
    }

    /*
    private void StartTutorialOld()
    {

        var audioPlayer = GetComponent<AudioPlayer>();

        audioPlayer.Play("tutorial_greeting", () =>
        {
            audioPlayer.Play("tutorial_trajectory_intro");

            IDisposable padObserver = null;

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
    */

    void StartTutorial()
    {

        IDisposable padObserver = null;

        // Teleport intro
        padObserver = this.UpdateAsObservable()
           .Where(_ => _padAction.GetStateUp(SteamVR_Input_Sources.RightHand)) // Input.GetKeyUp(KeyCode.A)
           .Subscribe(_ =>
           {
               padObserver.Dispose();
               m_TutorialPoster1.GetComponent<Negi.Outline>().enabled = false; // Remove outline from signboard
               m_TutorialPoster2.GetComponent<Negi.Outline>().enabled = true;
           }
           );

    }

}
