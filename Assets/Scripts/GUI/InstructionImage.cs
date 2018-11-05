using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InstructionImage : MonoBehaviour
{

    public Vector2 m_ExpandedImageSize = new Vector2(1024, 720);

    [SerializeField]
    private Transform m_HolderTransform;

    [SerializeField]
    private bool m_IsFacingPlayer = true;
    public bool facePlayer {
        get { return m_IsFacingPlayer; }
        set { m_IsFacingPlayer = value; }
    }
    
    private void OnEnable()
    {
        RectTransform rTransform = GetComponent<RectTransform>();
        rTransform.DOComplete();
        rTransform.sizeDelta = Vector2.zero;
        rTransform.DOSizeDelta(m_ExpandedImageSize, 1).SetEase(Ease.OutElastic);
    }
    
    private void Update()
    {
        if (facePlayer)
        {
            GetComponent<RectTransform>().LookAt(Camera.main.transform);
        }
    }

    public void SetImage(Sprite img)
    {
        transform.Find("Image").GetComponent<Image>().sprite = img;
    }

    public void MoveToHolder()
    {
        facePlayer = false;

        const float tweenDuration = 2;

        RectTransform rTransform = GetComponent<RectTransform>();

        transform.DOMove(m_HolderTransform.position, tweenDuration).SetEase(Ease.InBack);
        rTransform.DORotate(m_HolderTransform.eulerAngles, tweenDuration);
    }

}
