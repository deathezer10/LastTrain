﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Valve.VR;

public class PlayerDeathHandler : MonoBehaviour
{

    public ImageFader m_ImageFader;

    private static string m_GameOverTextKey = "death_timeup";

    public static string GameOverTextKey {
        get { return m_GameOverTextKey; }
    }

    public void KillPlayer(string gameOverTextKey)
    {
        m_ImageFader.DoFade(ImageFader.FadeType.ToOpaque, 1, () =>
        {
            m_GameOverTextKey = gameOverTextKey;
            SceneManager.LoadScene("GameOver");
        });
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S) || SteamVR_Input._default.inActions.GrabUse.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            KillPlayer("death_trainhit");
        }
    }

}