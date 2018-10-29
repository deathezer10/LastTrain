using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriver : GrabbableObject {

    private GameObject m_ScrewDriver;
    private BoxCollider m_Tip;
    GameObject ScrewDriverClone;



    private bool bIsGrabbing = false;
    public bool bIsScrewing = false;
    private float speed = 1.0f;
    // Use this for initialization

    void Start () {
        m_ScrewDriver = transform.gameObject;
        m_Tip = transform.GetChild(0).GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		
        if(bIsScrewing)
        {
            ScrewDriverClone.transform.Rotate(new Vector3(0,0,1), speed);
        }
	}


    public override void OnControllerEnter(PlayerViveController currentController)
    {
        
    }

    public override void OnControllerExit()
    {
        
    }

    public override void OnControllerStay()
    {
        
    }

    public override void OnGrab()
    {
        m_ScrewDriver.GetComponent<BoxCollider>().isTrigger = true;
        ScrewDriverClone = (GameObject)Instantiate(m_ScrewDriver, transform.position, transform.rotation,m_ScrewDriver.transform);
        Destroy(ScrewDriverClone.GetComponent("ScrewDriver"));

        bIsGrabbing = true;
    }

    public override void OnGrabReleased()
    {
        Destroy(ScrewDriverClone);
        m_ScrewDriver.GetComponent<BoxCollider>().isTrigger = false;
        bIsGrabbing = false;
    }

    public override void OnUse()
    {
      
    }

    public override void OnUseDown()
    {
        if (bIsGrabbing)
            bIsScrewing = true;
    }

    public override void OnUseUp()
    {
        bIsScrewing = false;
    }

}
