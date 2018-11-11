﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandleHolder : MonoBehaviour
{
    
    EmergencyHandlePanel panel;
    void Awake()
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

        handle.transform.position = transform.position;
        handle.transform.rotation = transform.rotation;

        GetComponent<AudioPlayer>().Play("handleclicked");

        handle.inserted = true;
    }

}
