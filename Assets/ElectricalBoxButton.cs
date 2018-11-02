using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElectricalBoxButton : MonoBehaviour
{

    private const float m_ToggleOffset = 0.02f;
    private AudioPlayer Audio;
    private bool bDisable = false;

    // Use this for initialization
    void Start()
    {
        Audio = transform.GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Controller"))
        if (!bDisable)
        { 
            bDisable = true;
            Audio.Play();
            transform.DOLocalMoveX(m_ToggleOffset, 0.09f).SetRelative();
            FindObjectOfType<TrainDoorHandler>().ToggleDoors(true);
            }
    }

}
