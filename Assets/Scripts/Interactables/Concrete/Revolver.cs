using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GrabbableObject
{

    [SerializeField]
    GameObject m_LaserPointer;

    [SerializeField]
    GameObject m_BarrelSmokeParticle;

    [SerializeField]
    GameObject m_BulletImpactParticle;

    private float m_PointerDistance;

    Vector3 m_OriginalLocalPosition;
    Vector3 m_OriginalScale;

    bool m_IsGrabbing = false;

    PlayerViveController m_CurrentController;

    GameObject m_CurrentPointedObject;

    RaycastHit? m_CurrentHitInfo;

    int m_CurrentBulletCount = 3;

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
        m_IsGrabbing = false;
        m_LaserPointer.SetActive(false);
        m_CurrentController.ToggleControllerModel(true);
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
            m_CurrentHitInfo = hitInfo;

            if (hitInfo.transform.GetComponent<IShootable>() != null)
            {
                m_CurrentPointedObject = hitInfo.transform.gameObject;
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
            m_CurrentHitInfo = null;
        }
    }

    public override void OnGrab()
    {
        m_IsGrabbing = true;
        m_LaserPointer.SetActive(true);
        transform.rotation = m_CurrentController.transform.rotation;
        transform.Rotate(new Vector3(0, -90, -15));
        transform.position = m_CurrentController.transform.position;
        m_CurrentController.ToggleControllerModel(false);
    }

    public override void OnGrabReleased()
    {
        m_IsGrabbing = false;
        m_LaserPointer.SetActive(false);
        m_CurrentController.ToggleControllerModel(true);
    }

    public override void OnUse()
    {
        if (m_CurrentBulletCount > 0)
        {
            m_CurrentBulletCount--;

            if (m_CurrentController.GetComponent<IShootable>() != null)
                m_CurrentPointedObject.GetComponent<IShootable>().OnShot(this);

            Instantiate(m_BarrelSmokeParticle, m_LaserPointer.transform.parent);

            if (m_CurrentHitInfo.HasValue)
            {
                Instantiate(m_BulletImpactParticle, m_CurrentHitInfo.Value.point, Quaternion.LookRotation(m_CurrentHitInfo.Value.normal));
                Debug.Log("eyy");
            }

            // Play firing sound
            GetComponent<AudioPlayer>().Play("bulletfire");
        }
        else
        {
            // Play no bullet sound
            GetComponent<AudioPlayer>().Play("bulletnone");
        }
    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }
}
