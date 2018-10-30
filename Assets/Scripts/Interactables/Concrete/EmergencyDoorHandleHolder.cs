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

        PlayerViveController.GetControllerThatHolds(handle.gameObject).DetachCurrentObject(false);

        Destroy(handle.GetComponent<EmergencyDoorHandle>());
        handle.GetComponent<Rigidbody>().useGravity = false;
        handle.GetComponent<Rigidbody>().isKinematic = true;

        handle.transform.position = m_HandleSnapPoint.position;
    }

}
