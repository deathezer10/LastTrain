using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrainArriver : MonoBehaviour
{
    [SerializeField]
    private TrainDoorHandler m_TrainDoor;

    [SerializeField]
    private List<Collider> _colliders = new List<Collider>();

    const float m_TrainStoppingPoint = 18f;

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

        var tweener = transform.DOMoveZ(m_TrainStoppingPoint, 10).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            bHasArrived = true;
            m_TrainDoor.ToggleDoors(true);
            onComplete?.Invoke();
        });

        var audio = GetComponent<AudioSource>();
        tweener.OnUpdate(() =>
        {
            if (tweener.ElapsedPercentage() >= 0.5f && !m_IsAudioFading)
            {
                audio?.DOFade(0, 3);
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

    private void DisabledCollider()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
    }
}
