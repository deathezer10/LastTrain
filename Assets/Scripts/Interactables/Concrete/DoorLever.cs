using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorLever : MonoBehaviour
{

    private PlayerViveController[] foundControllers;
    private GameObject parent;
    private BoxCollider ButtonCollider;
    private const float m_ToggleOffset = -0.02f;
    private AudioPlayer Audio;
    private bool bDisable = false;

   
    void Start()
    {
        foundControllers = FindObjectsOfType<PlayerViveController>();
        parent = transform.root.gameObject;
        ButtonCollider = GetComponent<BoxCollider>();
        Audio = GetComponent<AudioPlayer>();
    }

    
    void Update()
    {


    }


    private void OnTriggerEnter(Collider other)
    {
        if (!bDisable)
            if (other.gameObject.name == "Umbrella" || other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)")
            {
                bDisable = true;
                Audio.Play();
                DriverCabinDoorLock.init();
                transform.DOLocalMoveX(m_ToggleOffset, 0.09f).SetRelative();
            }
    }
}
