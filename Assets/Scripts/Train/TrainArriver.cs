using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour {

    public const float m_TrainStoppingPoint = 7.4f;

    public void BeginArrival()
    {
        transform.DOMoveX(m_TrainStoppingPoint, 5);
    }

}
