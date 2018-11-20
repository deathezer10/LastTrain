using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EscapeTunnelDoor : MonoBehaviour
{

    public void OpenEscapeDoor()
    {
        GetComponent<AudioPlayer>().Play();
        transform.DOLocalRotate(new Vector3(0, -40, 0), 5f, RotateMode.LocalAxisAdd);
    }
    
}
