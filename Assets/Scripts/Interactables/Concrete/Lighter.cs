﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : GrabbableObject
{
    [SerializeField]
    private ParticleSystem m_particle;

    AudioPlayer useAudio;
    CapsuleCollider litCollider;
    bool lit;
    private int count = 0;

    private void Start()
    {
        litCollider = GetComponent<CapsuleCollider>();
        useAudio = GetComponent<AudioPlayer>();
        m_particle.Stop();
    }

    public override bool hideControllerOnGrab { get { return true; } }

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
            useAudio.Play();
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
        if(other.name == "Bomb")
        {
            StartCoroutine(Counter());
        }

        if (lit)
        {
            if (other.tag == "PaperBurnArea")
            {
                other.GetComponent<NPBurnArea>().IncreaseHeat();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Bomb")
        count = 0;
    }

  private IEnumerator Counter()
    {
        yield return new WaitForSeconds(1);
        count++;
        if(count >= 3)
        {
            FindObjectOfType<Bomb>().TimerTimeOut();
        }
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

}
