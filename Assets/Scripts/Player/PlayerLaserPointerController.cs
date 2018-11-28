using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserPointerController : MonoBehaviour
{

    [SerializeField]
    private Valve.VR.SteamVR_Input_Sources m_CurrentHand;

    private float m_PointerDistance;

    Vector3 m_OriginalLocalPosition;
    Vector3 m_OriginalScale;

    VRGUIBase m_SelectedGUI;

    PlayerViveController[] m_ViveControllers;

    private void Start()
    {
        m_OriginalLocalPosition = transform.localPosition;
        m_OriginalScale = transform.localScale;
        m_PointerDistance = transform.localScale.z;
        m_ViveControllers = FindObjectsOfType<PlayerViveController>();
    }

    private void Update()
    {
        if (m_ViveControllers.Length > 0)
        {
            foreach (var controller in m_ViveControllers)
            {
                if (controller.GetCurrentHand() == m_CurrentHand)
                {
                    if (controller.GetCurrentHandObject() != null)
                    {
                        GetComponent<MeshRenderer>().enabled = false;
                        return;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }

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