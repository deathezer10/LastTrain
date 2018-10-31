using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class PlayerOriginHandler : MonoBehaviour
{

    private Material m_CurrentMaterial;

    private static bool m_IsOutsideOrigin = false;
    public static bool IsOutsideOrigin {
        get { return m_IsOutsideOrigin; }
    }

    public TextMeshPro[] m_MoveBackTextMesh;

    private void Start()
    {
        m_CurrentMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        ToggleMoveBackText(m_IsOutsideOrigin);
    }

    private void ToggleMoveBackText(bool enabled)
    {
        foreach (TextMeshPro text in m_MoveBackTextMesh)
        {
            text.gameObject.SetActive(enabled);
        }
    }

   

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "HeadDisplay")
        {
            Color currentColor = m_CurrentMaterial.GetColor("_TintColor");

            if (currentColor.a > 0)
            {
                currentColor.a -= Time.deltaTime;
                m_CurrentMaterial.SetColor("_TintColor", currentColor);
            }

            m_IsOutsideOrigin = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HeadDisplay")
        {
            Color currentColor = m_CurrentMaterial.GetColor("_TintColor");
            currentColor.a = 1;
            m_CurrentMaterial.SetColor("_TintColor", currentColor);

            m_IsOutsideOrigin = true;
        }
    }

}
