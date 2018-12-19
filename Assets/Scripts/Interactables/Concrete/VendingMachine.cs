using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class VendingMachine : MonoBehaviour
{
    [SerializeField]
    Transform m_FlapTransform;

    [SerializeField]
    Transform m_CanSpawnTrans;

    [SerializeField]
    float m_CurrentCredit;

    [SerializeField]
    GameObject m_DrinkCanPrefab;

    [SerializeField]
    Vector3 m_FlapEndRot;

    [SerializeField]
    Vector3 m_FlapEndPos;

    [SerializeField]
    Vector3 m_CanLaunchForce;

    [SerializeField]
    Material m_VMBlueMaterial;

    [SerializeField]
    Material m_VMRedMaterial;

    [SerializeField]
    bool m_bIsBlueVM;

    [SerializeField]
    TextMeshPro m_CreditTextMesh;

    Vector3 m_FlapStartRot;
    Vector3 m_FlapStartPos;

    void Start()
    {
        m_CreditTextMesh.text = "" + m_CurrentCredit;

        m_FlapStartRot = m_FlapTransform.localEulerAngles;
        m_FlapStartPos = m_FlapTransform.localPosition;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            VMButtonPress();
        }
    }

    /// <summary>
    /// Check if there is credit left in the machine, spawn a can if there is.
    /// </summary>
    public void VMButtonPress()
    {
        if (m_CurrentCredit >= 100f)
        {
            m_CurrentCredit = m_CurrentCredit - 100f;

            if (m_CurrentCredit < 100f)
            {
                if (m_bIsBlueVM)
                {
                    m_VMBlueMaterial.DisableKeyword("_EMISSION");
                }
                else
                {
                    m_VMRedMaterial.DisableKeyword("_EMISSION");
                }
            }

            m_CreditTextMesh.text = "" + m_CurrentCredit;

            StartCoroutine(FlapAction());
        }
    }

    private IEnumerator FlapAction()
    {
        yield return new WaitForSeconds(0.5f);

        m_FlapTransform.DOLocalRotate(m_FlapEndRot, 0.15f, RotateMode.Fast);
        m_FlapTransform.DOLocalMove(m_FlapEndPos, 0.15f, false);
        m_FlapTransform.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(0.20f);

        GameObject can = Instantiate(m_DrinkCanPrefab, m_CanSpawnTrans.position, m_CanSpawnTrans.rotation, null);

        if (can.GetComponent<Rigidbody>() != null)
        {
            can.GetComponent<Rigidbody>().AddForce(m_CanLaunchForce);
        }

        yield return new WaitForSeconds(0.2f);
        m_FlapTransform.DOLocalRotate(m_FlapStartRot, 0.15f, RotateMode.Fast);
        m_FlapTransform.DOLocalMove(m_FlapStartPos, 0.15f, false);
        m_FlapTransform.GetComponent<BoxCollider>().enabled = true;
    }
}
