using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GrabbableObject
{

    [SerializeField]
    GameObject m_LaserPointer;

    private float m_PointerDistance;

    Vector3 m_OriginalLocalPosition;
    Vector3 m_OriginalScale;

    bool m_IsGrabbing = false;

    private void Start()
    {
        m_LaserPointer.SetActive(false);
        m_OriginalLocalPosition = m_LaserPointer.transform.localPosition;
        m_OriginalScale = m_LaserPointer.transform.localScale;
        m_PointerDistance = m_LaserPointer.transform.localScale.z;
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

        var origin = m_LaserPointer.transform.parent.position;
        var dir = m_LaserPointer.transform.TransformDirection(-Vector3.forward);

        if (Physics.Raycast(origin, dir, out hitInfo, m_PointerDistance))
        {
            m_LaserPointer.transform.position = Vector3.Lerp(hitInfo.point, m_LaserPointer.transform.parent.position, 0.5f);

            Vector3 newScale = m_LaserPointer.transform.localScale;
            newScale.z = Mathf.Abs(m_LaserPointer.transform.localPosition.z * 2);
            m_LaserPointer.transform.localScale = newScale;
        }
        else
        {
            m_LaserPointer.transform.localPosition = m_OriginalLocalPosition;
            m_LaserPointer.transform.localScale = m_OriginalScale;
        }
    }

    public override void OnGrab()
    {
        m_IsGrabbing = true;
        m_LaserPointer.SetActive(true);
    }

    public override void OnGrabReleased()
    {
        m_IsGrabbing = false;
        m_LaserPointer.SetActive(false);
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
