using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandleHolder : MonoBehaviour
{

    [SerializeField]
    Transform m_HandleSnapPoint;


    private void OnTriggerEnter(Collider other)
    {
        EmergencyDoorHandle holder = other.GetComponent<EmergencyDoorHandle>();

        if (holder != null)
        {
            InsertHandle(holder);
        }
    }

    public void InsertHandle(EmergencyDoorHandle handle)
    {
        Debug.Log("Attached");

        var controller = PlayerViveController.GetControllerThatHolds(handle.gameObject);

        if (controller != null)
            controller.DetachCurrentObject(false);

        Destroy(handle.GetComponent<EmergencyDoorHandle>());
        Destroy(handle.GetComponent<Rigidbody>());
        Destroy(handle.GetComponent<Collider>());

        handle.transform.position = m_HandleSnapPoint.position;

        GetComponent<AudioPlayer>().Play("handleclicked");
    }

}
