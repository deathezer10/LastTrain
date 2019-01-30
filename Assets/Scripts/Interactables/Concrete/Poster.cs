using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Poster : StationaryObject
{

    private void Start()
    {

        var startRotate = this.transform.localEulerAngles;
        Debug.Log($"Rot : {startRotate}");
        Sequence sequence = DOTween.Sequence()
            .Append(this.transform.DORotate(startRotate + new Vector3(0f, 0f, 15.0f), 3f).SetEase(Ease.OutSine))
            .Append(this.transform.DORotate(startRotate, 3f).SetEase(Ease.OutSine));

        sequence.Play().SetLoops(-1, LoopType.Restart);
    }
}
