using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICCard : TutorialGrabbableObject
{
    protected override void Awake()
    {
        base.Awake();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Plastic });
    }
}
