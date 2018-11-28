using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorLever : MonoBehaviour, IShootable
{
    private const float m_ToggleOffset = -0.02f;
    private AudioPlayer Audio;
    private bool bDisable = false;


    void Start()
    {
        Audio = GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bDisable)
            if (other.gameObject.name == "Umbrella" || other.tag == "BaseballBat" ||  ((other.gameObject.name == "Controller (left)" || other.gameObject.name == "Controller (right)") && !PlayerOriginHandler.IsOutsideOrigin))
            {
                UnlockDoor();
            }
    }

    private void UnlockDoor()
    {
        bDisable = true;
        Audio.Play();
        DriverCabinDoorLock.init();
        FindObjectOfType<KeyCardScanner>().SetGreen();
        transform.DOLocalMoveX(m_ToggleOffset, 0.09f).SetRelative();
    }

    public void OnShot(Revolver revolver)
    {
        UnlockDoor();
    }

}
