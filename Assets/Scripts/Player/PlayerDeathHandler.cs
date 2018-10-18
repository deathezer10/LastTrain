using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Valve.VR;

public class PlayerDeathHandler : MonoBehaviour
{

    public Image m_FadeImage;

    public void KillPlayer()
    {
        m_FadeImage.DOFade(1, 1).OnComplete(() =>
        {
            SceneManager.LoadScene("GameOver");
        });
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S) ||SteamVR_Input._default.inActions.GrabUse.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            KillPlayer();
        }
    }

}