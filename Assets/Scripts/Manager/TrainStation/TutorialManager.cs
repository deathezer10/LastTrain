using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{

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
        });

    }

}
