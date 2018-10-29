using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriverTip : MonoBehaviour
{
    private BoxCollider ColliderTip;
    private ScrewDriver m_ScrewDriver;
    private float speed = 1.0f;
   
    // Use this for initialization
    void Start()
    {
        ColliderTip = transform.GetComponent<BoxCollider>();
        m_ScrewDriver = transform.parent.GetComponent<ScrewDriver>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Screw")
        {
            if(m_ScrewDriver.bIsScrewing)
            {
                other.transform.parent.Rotate(new Vector3(0, 0, -2), speed);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }


}
