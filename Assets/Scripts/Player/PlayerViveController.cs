using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

[RequireComponent(typeof(Collider))]
public class PlayerViveController : MonoBehaviour
{

    public enum HandSource
    {
        LeftHand,
        RightHand
    }

    public HandSource m_HandSource;

    private GameObject m_PrevGrabbedObj = null;

    private void OnTriggerEnter(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerEnter(m_HandSource);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerStay();

            var grabbableObject = other.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                if (m_PrevGrabbedObj == null && SteamVR_Input._default.inActions.GrabPinch.GetStateDown(HandSourceToInputSource()))
                {
                    grabbableObject.OnGrab();

                    m_PrevGrabbedObj = other.gameObject;

                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.velocity = Vector3.zero;

                    other.transform.parent = transform;

                    Debug.Log(m_HandSource.ToString() + " grabbed: " + grabbableObject.ToString());
                }

                if (m_PrevGrabbedObj != null && SteamVR_Input._default.inActions.GrabPinch.GetStateUp(HandSourceToInputSource()))
                {
                    grabbableObject.OnGrabReleased();

                    m_PrevGrabbedObj = null;

                    Rigidbody rb = other.GetComponent<Rigidbody>();
                    rb.useGravity = true;
                    rb.velocity = GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
                    rb.angularVelocity = GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();

                    other.transform.parent = null;

                    Debug.Log(m_HandSource.ToString() + " released: " + grabbableObject.ToString());
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerExit();
        }
    }

    private SteamVR_Input_Sources HandSourceToInputSource()
    {
        return (m_HandSource == HandSource.LeftHand) ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
    }

}
