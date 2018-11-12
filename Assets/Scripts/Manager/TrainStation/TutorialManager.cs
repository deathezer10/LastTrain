using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class TutorialManager : MonoBehaviour
{

    public enum PosterState { None, Poster1, Poster2, Poster3 }

    [SerializeField]
    private bool m_TutorialEnabled = true;
    public bool tutorialEnabled {
        get { return m_TutorialEnabled; }
        set { m_TutorialEnabled = value; }
    }

    [SerializeField]
    private SteamVR_Action_Boolean _padAction;

    [SerializeField]
    private Negi.Outline m_TutorialPoster1;
    [SerializeField]
    private Negi.Outline m_TutorialPoster2;
    [SerializeField]
    private Negi.Outline m_TutorialPoster3;

    [SerializeField]
    private List<Material> m_TutorialPosterMaterials = new List<Material>();

    private int m_CurrentPosterMaterialIndex = 0;

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

        SetPoster(PosterState.Poster1);

        IDisposable padObserver = null;

        // Teleport intro
        padObserver = this.UpdateAsObservable()
           .Where(_ => _padAction.GetStateUp(SteamVR_Input_Sources.RightHand)) // Input.GetKeyUp(KeyCode.A)
           .Subscribe(_ =>
           {
               padObserver.Dispose();
               SetPoster(PosterState.Poster2);
           }
           );

    }

    public void SetPoster(PosterState poster)
    {
        m_TutorialPoster1.enabled = false;
        m_TutorialPoster2.enabled = false;
        m_TutorialPoster3.enabled = false;

        StopAllCoroutines();

        switch (poster)
        {
            case PosterState.None:
                break;
            case PosterState.Poster1:
                m_TutorialPoster1.enabled = true;
                StartCoroutine(StartPosterAnimation(PosterState.Poster1));
                break;
            case PosterState.Poster2:
                m_TutorialPoster2.enabled = true;
                StartCoroutine(StartPosterAnimation(PosterState.Poster1));
                StartCoroutine(StartPosterAnimation(PosterState.Poster2));
                break;
            case PosterState.Poster3:
                m_TutorialPoster3.enabled = true;
                StartCoroutine(StartPosterAnimation(PosterState.Poster1));
                StartCoroutine(StartPosterAnimation(PosterState.Poster2));
                StartCoroutine(StartPosterAnimation(PosterState.Poster3));
                break;
            default:
                Debug.LogError("Invalid poster assigned");
                break;
        }

    }

    IEnumerator StartPosterAnimation(PosterState target)
    {
        while (target != PosterState.None)
        {
            yield return new WaitForSeconds(1);

            m_CurrentPosterMaterialIndex = (int)Mathf.Repeat(++m_CurrentPosterMaterialIndex, m_TutorialPosterMaterials.Count);

            var posterMat = m_TutorialPosterMaterials[m_CurrentPosterMaterialIndex];

            switch (target)
            {
                case PosterState.None:
                    break;
                case PosterState.Poster1:
                    m_TutorialPoster1.GetComponent<Renderer>().material = posterMat;
                    break;
                case PosterState.Poster2:
                    m_TutorialPoster2.GetComponent<Renderer>().material = posterMat;
                    break;
                case PosterState.Poster3:
                    m_TutorialPoster3.GetComponent<Renderer>().material = posterMat;
                    break;
                default:
                    Debug.LogError("Invalid poster assigned");
                    break;
            }
        }
    }

}
