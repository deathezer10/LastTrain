using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandleHolder : MonoBehaviour
{

    [SerializeField]
    Transform m_HandleSnapPoint;
    EmergencyHandlePanel panel;
    void start()
    {
        panel = FindObjectOfType<EmergencyHandlePanel>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EmergencyDoorHandle holder = other.GetComponent<EmergencyDoorHandle>();

        if(panel.bIsOpened)
        if (holder != null)
        {
            InsertHandle(holder);
            panel.bIsLocked = true;
        }
    }

    public void InsertHandle(EmergencyDoorHandle handle)
    {
        var controller = PlayerViveController.GetControllerThatHolds(handle.gameObject);

        if (controller != null)
            controller.DetachCurrentObject(false);

        handle.SetHolder(this);

        handle.GetComponent<Rigidbody>().useGravity = false;
        handle.GetComponent<Rigidbody>().isKinematic = true;

        handle.transform.position = m_HandleSnapPoint.position;
        handle.transform.rotation = m_HandleSnapPoint.transform.rotation;

        GetComponent<AudioPlayer>().Play("handleclicked");

        handle.inserted = true;
    }

}
