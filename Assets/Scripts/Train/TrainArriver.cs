using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour
{

    public TrainDoorHandler m_TrainDoor;

    const float m_TrainStoppingPoint = -4;

    bool m_IsAudioFading = false;

    public void BeginArrival(System.Action onComplete = null)
    {
        GetComponent<AudioPlayer>().Play();

        var tweener = transform.DOMoveZ(m_TrainStoppingPoint, 10).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            m_TrainDoor.ToggleDoors(true);

            if (onComplete != null)
                onComplete();
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

}
