using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandle : GrabbableObject
{
    private PlayerViveController m_Controller;

    private float m_LastZRot;

    private bool m_Inserted = false;
    public bool inserted {
        get { return m_Inserted; }
        set { m_Inserted = value; }
    }

    private bool m_Grabbing = false;

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        m_Controller = currentController;
    }

    public override void OnControllerStay()
    {
        if (m_Grabbing == false)
            m_LastZRot = m_Controller.transform.rotation.z;
    }

    public override void OnGrab()
    {
        m_Grabbing = true;
    }

    public override void OnGrabReleased()
    {
        m_Grabbing = false;
    }

    public override void OnGrabStay()
    {
        if (m_Inserted == true && m_Controller != null)
        {
            float currentZRot = m_Controller.transform.rotation.z;
            float rotationDelta = currentZRot - m_LastZRot;

            if (m_LastZRot != currentZRot)
            {
                transform.Rotate(0, 0, rotationDelta);
            }

            Debug.LogFormat("Last Z: {0}\nCurrent Z: {1}\nDelta: {2}", m_LastZRot, currentZRot, rotationDelta);
        }
    }

}
