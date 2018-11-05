using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour
{

    public TrainDoorHandler m_TrainDoor;

    const float m_TrainStoppingPoint = 2;

    const float m_TrainArrivalDelay = 5;

    bool m_ArrivalTriggered = false;

    bool m_IsAudioFading = false;

    private bool bHasArrived = false;
    public bool HasArrived {
        get { return bHasArrived; }
    }

    public void BeginArrival(System.Action onComplete = null)
    {
        GetComponent<AudioPlayer>().Play();

        var tweener = transform.DOMoveZ(m_TrainStoppingPoint, 10).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            bHasArrived = true;
            m_TrainDoor.ToggleDoors(true);
            onComplete?.Invoke();
        });

        tweener.OnUpdate(() =>
        {
            if (tweener.ElapsedPercentage() >= 0.5f && !m_IsAudioFading)
            {
                GetComponent<AudioSource>().DOFade(0, 3);
                m_IsAudioFading = true;
            }
        });

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && m_ArrivalTriggered == false)
        {
            m_ArrivalTriggered = true;
            Invoke("BeginArrival", m_TrainArrivalDelay);
        }
    }

    public void CallTheTrain()
    {
        if (m_ArrivalTriggered == false)
        {
            m_ArrivalTriggered = true;
            Invoke("BeginArrival", m_TrainArrivalDelay);
        }
    }

}
