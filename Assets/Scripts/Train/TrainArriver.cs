using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour {

    public TrainDoorHandler m_TrainDoor;

    const float m_TrainStoppingPoint = 4.85f;

    public void BeginArrival()
    {
        transform.DOMoveZ(m_TrainStoppingPoint, 10).SetEase(Ease.OutQuart).OnComplete(()=> {
            m_TrainDoor.ToggleDoors(true);
        });
    }

}
