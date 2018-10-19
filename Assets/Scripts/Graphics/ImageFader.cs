using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ImageFader))]
public class ImageFader : MonoBehaviour
{

    private Image m_FaderImage;

    public enum FadeType
    {
        ToOpaque,
        ToInvisible,
    }

    private void Start()
    {
        m_FaderImage = GetComponent<Image>();
    }

    public void DoFade(FadeType fadeType, float duration, System.Action onComplete)
    {
        if (fadeType == FadeType.ToInvisible)
        {
            
        }
        else if (fadeType == FadeType.ToOpaque)
        {

        }
    }

}
