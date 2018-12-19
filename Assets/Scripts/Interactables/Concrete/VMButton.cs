using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VMButton : MonoBehaviour
{

    [SerializeField]
    Color m_UseFlashColor;

    [SerializeField]
    Color m_OriginalColor;

    [SerializeField]
    AudioPlayer m_ButtonUseAudio;

    float m_UseCooldown;

    void Start()
    {
        m_UseCooldown = Time.time + 1.2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {
            if (m_UseCooldown >= Time.time)
            {
                StartCoroutine(ButtonPressRoutine());
                GetComponent<VendingMachine>().VMButtonPress();
            }
        }
    }

    private IEnumerator ButtonPressRoutine()
    {
        m_UseCooldown = Time.time + 1.2f;

        m_ButtonUseAudio.Play();

        GetComponent<Material>().SetColor("_Emission", m_UseFlashColor);

        yield return new WaitForSeconds(0.2f);

        GetComponent<Material>().SetColor("_Emission", m_OriginalColor);
    }
}
