using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandle : GrabbableObject
{
    private PlayerViveController m_Controller;
    private TrainDoorHandler trainDoorHandler; 
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

    private EmergencyDoorHandleHolder m_HandleHolder;

    private void Start()
    {
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Metal });
        trainDoorHandler = FindObjectOfType<TrainDoorHandler>();
    }

    public void SetHolder(EmergencyDoorHandleHolder holder)
    {
        m_HandleHolder = holder;
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        m_Controller = currentController;
    }

    public override void OnControllerStay()
    {
        base.OnControllerStay();

        if (m_Grabbing == false && m_Controller != null)
            m_LastZRot = m_Controller.transform.eulerAngles.z;
    }

    public override void OnGrab()
    {
        base.OnGrab();

        var rb = GetComponent<Rigidbody>();

        rb.useGravity = true;

        m_Grabbing = true;
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        var rb = GetComponent<Rigidbody>();

        rb.useGravity = true;

        m_Grabbing = false;
    }

    public override void OnGrabStay()
    {
        base.OnGrabStay();

        if (m_Locked == false && m_Inserted == true && m_Controller != null)
        {
            float currentZRot = m_Controller.transform.eulerAngles.z;
            float rotationDelta = currentZRot - m_LastZRot;

            if (m_LastZRot != currentZRot)
            {
                transform.Rotate(rotationDelta, 0, 0);
                m_HandleHolder.transform.Rotate(rotationDelta, 0, 0);
                m_LastZRot = currentZRot;
                m_TotalRotation += Mathf.DeltaAngle(rotationDelta, 0);

                // Emergency door unleashed
                if (m_TotalRotation >= 90 || m_TotalRotation <= -90)
                {
                    transform.eulerAngles = new Vector3((m_TotalRotation >= 90) ? 90 : -90, 0, 0);
                    m_HandleHolder.transform.eulerAngles = transform.eulerAngles;

                    m_Locked = true;
                    GetComponent<AudioPlayer>().Play("leverlocked");
                    if(!trainDoorHandler.bAreDoorsOpen)
                    FindObjectOfType<TrainDoorHandler>().ToggleDoors(true);

                    FindObjectOfType<EmergencyDoorsManager>().EmergencyDoorsTriggered();

                    var doorSound = FindObjectOfType<TrainDoorsOpenSound>();

                    if (doorSound != null)
                        doorSound.CabinDoorsPlay("Cabin1");
                }

            }
        }
    }

}
