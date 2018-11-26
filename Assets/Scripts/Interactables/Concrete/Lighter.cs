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
    bool bombed = false;
    private int count = 0;

    PlayerViveController m_CurrentController;

    private void Start()
    {
        litCollider = GetComponent<CapsuleCollider>();
        litCollider.enabled = false;
        useAudio = GetComponent<AudioPlayer>();
        m_particle.Stop();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Plastic });
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        m_CurrentController = currentController;
    }

    public override void OnGrab()
    {
        base.OnGrab();

        if (m_CurrentController != null)
        {
            transform.position = m_CurrentController.transform.position;
            transform.rotation = m_CurrentController.transform.rotation;
            transform.Rotate(new Vector3(90, 0, 0), Space.Self);
        }
    }

    public override void OnUse()
    {
        base.OnUse();

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
        if (lit)
        {
            if (other.name == "Bomb" && !bombed)
            {
                Bomb bomb = other.GetComponent<Bomb>();
                if (bomb.isGlassBroken)
                {
                    bomb.TimerTimeOut();
                    bombed = true;
                }
            }

            if (other.tag == "PaperBurnArea")
            {
                other.GetComponent<NPBurnArea>().IncreaseHeat();
            }
            else if (other.tag == "DollBurnArea")
            {
                other.GetComponent<DollBurnArea>().IncreaseHeat();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Bomb")
            count = 0;
    }

    private IEnumerator Counter()
    {
        yield return new WaitForSeconds(1);
        count++;
        if (count >= 3)
        {
            FindObjectOfType<Bomb>().TimerTimeOut();
        }
    }
}
