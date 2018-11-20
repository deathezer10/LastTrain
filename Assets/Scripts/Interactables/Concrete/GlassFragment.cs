using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassFragment : GrabbableObject
{

    private void Start()
    {
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Glass });
    }

    public override void OnGrab()
    {
        base.OnGrab();

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshCollider>().enabled = false;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshCollider>().enabled = true;
    }

}
