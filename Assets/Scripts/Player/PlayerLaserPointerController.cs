using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserPointerController : MonoBehaviour
{

    const float m_PointerDistance = 10;

    Vector3 m_OriginalPosition;
    Vector3 m_OriginalScale;

    private void Start()
    {
        m_OriginalPosition = transform.position;
        m_OriginalScale = transform.localScale;
    }

    private void Update()
    {
        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, -transform.right, Color.yellow);

        if (Physics.Raycast(transform.position, -transform.right, out hitInfo, m_PointerDistance))
        {
            VRGUIBase guiBase = hitInfo.transform.GetComponent<VRGUIBase>();

            if (guiBase != null)
            {
                guiBase.OnPointerStay();
            }
            
            transform.position = (hitInfo.point - transform.position) / 2;
            
            Vector3 newScale = transform.localScale;
            newScale.z = transform.position.z * 2;
            transform.localScale = newScale;
        }
        else
        {
            transform.position = m_OriginalPosition;
            transform.localScale = m_OriginalScale;
        }
    }

}