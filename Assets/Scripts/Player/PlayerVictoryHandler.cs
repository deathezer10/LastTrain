using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Valve.VR;

public class PlayerVictoryHandler : MonoBehaviour
{

    public ImageFader victoryFader;

    public void PlayerVictory()
    {
        victoryFader.FadeIn(2, () =>
        {
            SceneManager.LoadScene("Victory");
        });
    }

}