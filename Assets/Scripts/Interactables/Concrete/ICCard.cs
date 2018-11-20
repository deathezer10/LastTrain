using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICCard : GrabbableObject
{
    private Negi.Outline _outline;

    protected override void Awake() {
        base.Awake();
        _outline = GetComponent<Negi.Outline>();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Plastic });
    }

    private void LateUpdate()
    {
        _outline.enabled = true;
    }
}
