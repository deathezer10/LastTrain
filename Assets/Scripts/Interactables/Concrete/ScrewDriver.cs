using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriver : GrabbableObject
{

    private GameObject m_ScrewDriver;
    private GameObject ScrewDriverClone;
    private PlayerViveController Controller;


    private bool bIsGrabbing = false;
    public bool bIsScrewing = false;
    public float speed = 2.0f;
    private float RotationValue;
    // Use this for initialization

    private Screw[] m_Screws;

    void Start()
    {
        m_ScrewDriver = transform.gameObject;
        m_Screws = FindObjectsOfType<Screw>();
    }

    // Update is called once per frame
    void Update()
    {

        if (bIsGrabbing)
        {
            Quaternion temp = Quaternion.LookRotation(-Controller.transform.forward);
            ScrewDriverClone.transform.position = m_ScrewDriver.transform.position;
            ScrewDriverClone.transform.rotation = Quaternion.Euler(temp.eulerAngles.x, temp.eulerAngles.y, RotationValue);
        }

        if (bIsScrewing)
        {
            RotationValue += speed;
        }

    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        Controller = currentController;
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();

        m_ScrewDriver.GetComponent<MeshRenderer>().enabled = true;
        Destroy(ScrewDriverClone);
        bIsGrabbing = false;
        bIsScrewing = false;
    }

    public override void OnGrab()
    {
        base.OnGrab();

        m_ScrewDriver.transform.position = Controller.transform.position;
        m_ScrewDriver.transform.rotation = Quaternion.LookRotation(-Controller.transform.forward);
        ScrewDriverClone = Instantiate(m_ScrewDriver, transform.position, transform.rotation, m_ScrewDriver.transform);
        base.OnGrabStay();
        m_ScrewDriver.GetComponent<MeshRenderer>().enabled = false;
        Destroy(ScrewDriverClone.GetComponent("ScrewDriver"));
        ScrewDriverClone.GetComponent<Rigidbody>().useGravity = false;
        bIsGrabbing = true;
        RotationValue = ScrewDriverClone.transform.rotation.eulerAngles.z;

        foreach (Screw screw in m_Screws)
        {
            Negi.Outline outline = screw.GetComponent<Negi.Outline>();

            if (outline == null && Vector3.Distance(transform.position, screw.transform.position) < 4)
                screw.gameObject.AddComponent<Negi.Outline>();
            else
                outline.enabled = true;
        }
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();

        m_ScrewDriver.GetComponent<MeshRenderer>().enabled = true;
        Destroy(ScrewDriverClone);
        bIsGrabbing = false;

        foreach (Screw screw in m_Screws)
        {
            Negi.Outline outline = screw.GetComponent<Negi.Outline>();

            if (outline != null)
                outline.enabled = false;
        }
    }

}
