using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VMButton : MonoBehaviour
{

    [SerializeField]
    Color m_UseFlashColor;

    Color m_OriginalColor;

    float m_UseCooldown;

    void Start()
    {
        m_OriginalColor = GetComponent<Material>().GetColor("_Emission");

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
        
        GetComponent<Material>().SetColor("_Emission", m_UseFlashColor);

        yield return new WaitForSeconds(0.2f);

        GetComponent<Material>().SetColor("_Emission", m_OriginalColor);
    }
}
