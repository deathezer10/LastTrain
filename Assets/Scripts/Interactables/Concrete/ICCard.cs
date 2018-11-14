using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICCard : GrabbableObject
{
    private Negi.Outline _outline;
    private void Start() {
        _outline = this.GetComponent<Negi.Outline>();
    }
    private void LateUpdate()
    {
        _outline.enabled = true;
    }
}
