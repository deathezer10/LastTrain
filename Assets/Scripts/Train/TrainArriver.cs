using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour {

    public const float m_TrainStoppingPoint = 4.85f;

    public void BeginArrival()
    {
        transform.DOMoveZ(m_TrainStoppingPoint, 10).SetEase(Ease.OutQuart);
    }

}
