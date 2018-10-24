using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorLever : MonoBehaviour
{
    private const float m_ToggleOffset = -0.02f;
    private AudioPlayer Audio;
    private bool bDisable = false;


    void Start()
    {
        Audio = GetComponent<AudioPlayer>();
    }


    void Update()
    {


    }


    private void OnTriggerEnter(Collider other)
    {
        if (!bDisable)
            if (other.gameObject.name == "Umbrella" || ((other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)") && DriverCabinDoorLock.bIsUnlocked))
            {
                bDisable = true;
                Audio.Play();
                DriverCabinDoorLock.init();
                transform.DOLocalMoveX(m_ToggleOffset, 0.09f).SetRelative();
            }
    }
}
