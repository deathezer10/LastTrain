using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InstructionImage : MonoBehaviour
{

    public Vector2 m_ExpandedImageSize = new Vector2(1024, 720);

    private bool m_IsFacingPlayer;
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
            transform.rotation.SetLookRotation(Camera.main.transform.position - transform.position);
        }
    }

}
