using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour
{

    public TrainDoorHandler m_TrainDoor;

    const float m_TrainStoppingPoint = 6f;

    const float m_TrainArrivalDelay = 3f;

    bool m_ArrivalTriggered = false;

    bool m_IsAudioFading = false;

    private bool bHasArrived = false;
    public bool HasArrived {
        get { return bHasArrived; }
    }

    public IEnumerator BeginArrival(System.Action onComplete = null)
    {
        yield return new WaitForSeconds(m_TrainArrivalDelay);

        GetComponent<AudioPlayer>().Play();

        var tweener = transform.DOMoveZ(m_TrainStoppingPoint, 8).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            bHasArrived = true;
            //m_TrainDoor.ToggleDoors(true);
            FindObjectOfType<TrainDoorHandler>().ToggleDoors(true);
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

    public void CallTheTrain()
    {
        if (m_ArrivalTriggered == false)
        {
            m_ArrivalTriggered = true;
            StartCoroutine(BeginArrival());
        }
    }

}
