using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : GrabbableObject
{
    [SerializeField]
    private ParticleSystem m_particle;

    CapsuleCollider litCollider;
    bool lit;

    private void Start()
    {
        litCollider = GetComponent<CapsuleCollider>();
        m_particle.Stop();
    }

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
        if (lit)
        {
            m_particle.Stop();
            lit = false;
            litCollider.enabled = false;
        }
        else
        {
            m_particle.Play();
            lit = true;
            litCollider.enabled = true;
        }
    }

    public bool IsLit()
    {
        return lit;
    }

    private void OnTriggerStay(Collider other)
    {
        if (lit)
        {
            if (other.tag == "PaperBurnArea")
            {
                other.GetComponent<NPBurnArea>().IncreaseHeat();
            }
        }
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

}
