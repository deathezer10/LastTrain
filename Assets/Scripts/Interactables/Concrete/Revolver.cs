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

    PlayerViveController m_CurrentController;

    GameObject m_CurrentPointedObject;

    int m_CurrentBulletCount = 2;

    private void Start()
    {
        m_LaserPointer.SetActive(false);
        m_OriginalLocalPosition = m_LaserPointer.transform.localPosition;
        m_OriginalScale = m_LaserPointer.transform.localScale;
        m_PointerDistance = m_LaserPointer.transform.localScale.z;
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        m_CurrentController = currentController;
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

            if (hitInfo.transform.GetComponent<IShootable>() != null)
            {
                m_CurrentPointedObject = hitInfo.transform.gameObject;
                Debug.Log("Pointing at: " + m_CurrentPointedObject.name);
            }
            else
            {
                m_CurrentPointedObject = null;
            }
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
        transform.rotation = m_CurrentController.transform.rotation;
        transform.Rotate(new Vector3(0, -90, 25));
        transform.position = m_CurrentController.transform.position;
    }

    public override void OnGrabReleased()
    {
        m_IsGrabbing = false;
        m_LaserPointer.SetActive(false);
    }

    public override void OnUse()
    {
        if (m_CurrentBulletCount > 0)
        {
            m_CurrentBulletCount--;

            m_CurrentPointedObject.GetComponent<IShootable>().OnShot(this);
        }
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }
}
