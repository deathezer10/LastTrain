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
    private SteamVR_Action_Boolean _padAction;

    [SerializeField]
    private GameObject m_Wallet;

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<PlayerTeleportation>().enabled = false;
        Invoke("StartTutorial", 2);
    }

    void StartTutorial()
    {

        var audioPlayer = GetComponent<AudioPlayer>();

        audioPlayer.Play("tutorial_greeting", () =>
        {
            audioPlayer.Play("tutorial_trajectory_intro");

            System.IDisposable padObserver = null;

            // Teleport intro
            padObserver = this.UpdateAsObservable()
           .Where(_ => _padAction.GetStateDown(SteamVR_Input_Sources.Any))// Input.GetKeyDown(KeyCode.A))
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
                   padObserver.Dispose();
                   audioPlayer.Stop();
                   audioPlayer.Play("tutorial_wallet_intro");

                   // Wallet Intro will be in the Wallet.cs script
               });
           }
           );

        }, 0.5f);

    }

}
