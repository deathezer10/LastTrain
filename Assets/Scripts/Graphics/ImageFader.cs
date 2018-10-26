using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ImageFader : MonoBehaviour
{
    public bool m_FadeOutOnStart;
    private Image m_FaderImage;

    [SerializeField]
    private Color m_fadeColor;

    private void Start()
    {

        if (m_FadeOutOnStart)
        {
            FadeManager.Instance._fadeColor = m_fadeColor;
            StartCoroutine(FadeManager.Instance.FadeOut(1));
        }
    }

    public void FadeIn(float duration, System.Action onComplete = null)
    {
        FadeManager.Instance._fadeColor = m_fadeColor;
        StartCoroutine(FadeManager.Instance.FadeIn(duration,onComplete));
    }

}
