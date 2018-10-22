using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserPointerController : MonoBehaviour
{

    private float m_PointerDistance;

    Vector3 m_OriginalLocalPosition;
    Vector3 m_OriginalScale;

    VRGUIBase m_SelectedGUI;

    private void Start()
    {
        m_OriginalLocalPosition = transform.localPosition;
        m_OriginalScale = transform.localScale;
        m_PointerDistance = transform.localScale.z;
    }

    private void Update()
    {
        RaycastHit hitInfo;

        Debug.DrawRay(transform.parent.position, transform.forward, Color.yellow);

        if (Physics.Raycast(transform.parent.position, transform.forward, out hitInfo, m_PointerDistance))
        {
            VRGUIBase guiBase = hitInfo.transform.GetComponent<VRGUIBase>();

            if (guiBase != null)
            {
                if (m_SelectedGUI == null)
                {
                    m_SelectedGUI = guiBase;
                    guiBase.OnPointerEntered();
                }

                guiBase.OnPointerStay();
            }

            transform.position = Vector3.Lerp(hitInfo.point, transform.parent.position, 0.5f);

            Vector3 newScale = transform.localScale;
            newScale.z = transform.localPosition.z * 2;
            transform.localScale = newScale;
        }
        else
        {
            transform.localPosition = m_OriginalLocalPosition;
            transform.localScale = m_OriginalScale;

            if (m_SelectedGUI != null)
            {
                m_SelectedGUI.OnPointerExit();
                m_SelectedGUI = null;
            }

        }
    }

}