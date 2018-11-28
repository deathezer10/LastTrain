﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Valve.VR;

public class PlayerDeathHandler : MonoBehaviour
{

    public ImageFader m_DeathFader;
    public float FadeTime = 2;

    private static string m_GameOverTextKey = "death_timeup";

    private bool isDead = false;

    public static string GameOverTextKey {
        get { return m_GameOverTextKey; }
    }

    public void KillPlayer(string gameOverTextKey, bool fadeInstantly = false)
    {
        if (isDead == true)
            return;

        isDead = true;

        m_DeathFader.FadeIn((fadeInstantly) ? 0.1f : FadeTime, () =>
         {
             m_GameOverTextKey = gameOverTextKey;
             SceneManager.LoadScene("GameOver");
         });
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            KillPlayer("death_trainhit");
        }
    }

}