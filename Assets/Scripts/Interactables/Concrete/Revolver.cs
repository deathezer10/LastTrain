using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Revolver : GrabbableObject
{

    [SerializeField]
    GameObject m_BulletChamber;

    [SerializeField]
    GameObject m_LaserPointer;

    [SerializeField]
    GameObject m_BarrelSmokeParticle;

    [SerializeField]
    GameObject m_BulletImpactParticle;

    [SerializeField]
    GameObject m_LaserPointerBall;

    private float m_PointerDistance;

    Vector3 m_OriginalLocalPosition;
    Vector3 m_OriginalScale;

    bool m_IsGrabbing = false;

    PlayerViveController m_CurrentController;

    GameObject m_CurrentPointedObject;

    /// <summary>
    /// The point of impact where the bullet landed, null if it hit nothing
    /// </summary>
    public RaycastHit? hitInfo { get; private set; }

    int m_CurrentBulletCount = 99;

    private void Start()
    {
        m_LaserPointer.SetActive(false);
        m_OriginalLocalPosition = m_LaserPointer.transform.localPosition;
        m_OriginalScale = m_LaserPointer.transform.localScale;
        m_PointerDistance = m_LaserPointer.transform.localScale.z;
    }

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);
        m_CurrentController = currentController;
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
        
        m_IsGrabbing = false;
        m_LaserPointer.SetActive(false);
    }

    public override void OnControllerStay()
    {
        base.OnControllerStay();

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
            this.hitInfo = hitInfo;

            if (hitInfo.transform.GetComponent<IShootable>() != null)
            {
                m_CurrentPointedObject = hitInfo.transform.gameObject;
                m_LaserPointerBall.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            }
            else
            {
                m_CurrentPointedObject = null;
                m_LaserPointerBall.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            }
        }
        else
        {
            m_LaserPointer.transform.localPosition = m_OriginalLocalPosition;
            m_LaserPointer.transform.localScale = m_OriginalScale;
            this.hitInfo = null;
        }
    }

    public override void OnGrab()
    {
        base.OnGrab();

        m_IsGrabbing = true;
        m_LaserPointer.SetActive(true);
        transform.rotation = m_CurrentController.transform.rotation;
        transform.Rotate(new Vector3(0, -90, -15));
        transform.position = m_CurrentController.transform.position;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        m_IsGrabbing = false;
        m_LaserPointer.SetActive(false);
    }

    public override void OnUse()
    {
        if (m_CurrentBulletCount > 0)
        {
            m_CurrentBulletCount--;

            if (m_CurrentPointedObject != null && m_CurrentPointedObject.GetComponent<IShootable>() != null)
                m_CurrentPointedObject.GetComponent<IShootable>().OnShot(this);

            Instantiate(m_BarrelSmokeParticle, m_LaserPointer.transform.parent);

            if (hitInfo.HasValue)
                Instantiate(m_BulletImpactParticle, hitInfo.Value.point, Quaternion.LookRotation(hitInfo.Value.normal));

            m_BulletChamber.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.2f, RotateMode.LocalAxisAdd);

            // Play firing sound
            GetComponent<AudioPlayer>().Play("bulletfire");
        }
        else
        {
            // Play no bullet sound
            GetComponent<AudioPlayer>().Play("bulletnone");
        }
    }
}
