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

    private bool m_Locked = false;
    public bool locked {
        get { return m_Locked; }
        set { m_Locked = value; }
    }

    private bool m_Grabbing = false;

    const float m_ClickRotationThreshold = 90;
    float m_TotalRotation = 0;


    public override void OnControllerEnter(PlayerViveController currentController)
    {
        m_Controller = currentController;
    }

    public override void OnControllerStay()
    {
        if (m_Grabbing == false)
            m_LastZRot = m_Controller.transform.localEulerAngles.z;
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
        if (m_Locked == false && m_Inserted == true && m_Controller != null)
        {
            float currentZRot = m_Controller.transform.localEulerAngles.z;
            float rotationDelta = currentZRot - m_LastZRot;

            if (m_LastZRot != currentZRot)
            {
                transform.Rotate(rotationDelta, 0, 0);
                m_LastZRot = currentZRot;
                m_TotalRotation += rotationDelta;

                // Emergency door unleashed
                if (m_TotalRotation <= -m_ClickRotationThreshold || m_TotalRotation >= m_ClickRotationThreshold)
                {
                    m_Locked = true;
                    GetComponent<AudioPlayer>().Play("leverlocked");
                    FindObjectOfType<TrainDoorHandler>().ToggleDoors(true);
                }

            }
        }
    }

}
