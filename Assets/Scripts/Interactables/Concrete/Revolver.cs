using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GrabbableObject
{

    [SerializeField]
    GameObject m_LaserOrigin;

    private float m_PointerDistance;

    Vector3 m_OriginalLocalPosition;
    Vector3 m_OriginalScale;

    bool m_IsGrabbing = false;

    private void Start()
    {
        m_LaserOrigin.SetActive(false);
        m_OriginalLocalPosition = m_LaserOrigin.transform.localPosition;
        m_OriginalScale = m_LaserOrigin.transform.localScale;
        m_PointerDistance = m_LaserOrigin.transform.localScale.y;
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
    }

    public override void OnControllerExit()
    {
    }

    public override void OnControllerStay()
    {
        if (m_IsGrabbing == false)
            return;
        
        RaycastHit hitInfo;

        int mask = (1 << LayerMask.NameToLayer("Environment"));
        
        if (Physics.Raycast(m_LaserOrigin.transform.parent.position, m_LaserOrigin.transform.forward, out hitInfo, m_PointerDistance, mask))
        {
            m_LaserOrigin.transform.position = Vector3.Lerp(hitInfo.point, m_LaserOrigin.transform.parent.position, 0.5f);

            Vector3 newScale = m_LaserOrigin.transform.localScale;
            newScale.y = m_LaserOrigin.transform.localPosition.y * 2;
            m_LaserOrigin.transform.localScale = newScale;

            Debug.Log("Ray hit");
        }
        else
        {
            m_LaserOrigin.transform.localPosition = m_OriginalLocalPosition;
            m_LaserOrigin.transform.localScale = m_OriginalScale;
        }
    }

    public override void OnGrab()
    {
        m_IsGrabbing = true;
        m_LaserOrigin.SetActive(true);
    }

    public override void OnGrabReleased()
    {
        m_IsGrabbing = false;
        m_LaserOrigin.SetActive(false);
    }

    public override void OnUse()
    {
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }
}
