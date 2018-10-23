using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : GrabbableObject {
	[SerializeField]
	private ParticleSystem m_particle;

    public override void OnControllerEnter(PlayerViveController currentController)
    {
    }

    public override void OnControllerExit()
    {
    }

    public override void OnControllerStay()
    {
    }

    public override void OnGrab()
    {
		Debug.Log("ライターを持ったよ");

        transform.eulerAngles = Vector3.zero;
    }

    public override void OnGrabReleased()
    {
        Debug.Log("ライターを離したよ");
    }

    public override void OnUse()
    {
        Debug.Log("ライターを使ったよ");
        if (m_particle.isPlaying) m_particle.Stop();
        else m_particle.Play();
    }
}
