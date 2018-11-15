using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICCard : GrabbableObject
{
    private Negi.Outline _outline;

    [SerializeField]
    private int _outlineColor = 1;
    private void Awake() {
        _outline = this.GetComponent<Negi.Outline>();
        _outline._color = _outlineColor;
    }

    private void LateUpdate()
    {
        _outline.enabled = true;
    }
}
