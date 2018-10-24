using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class ImageFader : MonoBehaviour
{

    public bool m_FadeOutOnStart;
    private Image m_FaderImage;

    public enum FadeType
    {
        ToOpaque,
        ToInvisible,
    }

    private void Start()
    {
        m_FaderImage = GetComponent<Image>();

        if (m_FadeOutOnStart)
        {
            m_FaderImage.DOFade(0, 1);
        }
    }

    public void DoFade(FadeType fadeType, float duration, System.Action onComplete = null)
    {

        m_FaderImage.DOFade((fadeType == FadeType.ToOpaque) ? 1 : 0, duration).OnComplete(() =>
        {
            if (onComplete != null) onComplete.Invoke();
        });

    }

}
