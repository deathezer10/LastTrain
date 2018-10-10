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

    public HandSource m_CurrentHand;

    private static GameObject m_CurrentLeftObject = null;
    private static GameObject m_CurrentRightObject = null;

    private void OnTriggerEnter(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerEnter(m_CurrentHand);
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

                // On Grab
                if (GetCurrentHandObject() == null)
                {
                    if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(HandSourceToInputSource()))
                    {
                        // If other hand is holding this object, unassign it
                        if (GetCurrentHandObject(true) != null && GetCurrentHandObject(true) == other.gameObject)
                        {
                            AssignObjectToHand(GetOtherHand(), null);
                        }

                        grabbableObject.OnGrab();

                        AssignObjectToHand(m_CurrentHand, other.gameObject);

                        Rigidbody rb = other.GetComponent<Rigidbody>();
                        rb.isKinematic = true;
                        rb.velocity = Vector3.zero;

                        other.transform.parent = transform;

                        Debug.Log(m_CurrentHand.ToString() + " grabbed: " + grabbableObject.ToString());
                    }
                }

                // On Grab Released
                if (GetCurrentHandObject() != null)
                {
                    if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(HandSourceToInputSource()))
                    {
                        grabbableObject.OnGrabReleased();

                        AssignObjectToHand(m_CurrentHand, null);

                        Rigidbody rb = other.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.velocity = GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
                        rb.angularVelocity = GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();

                        other.transform.parent = null;

                        Debug.Log(m_CurrentHand.ToString() + " released: " + grabbableObject.ToString());
                    }
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
        return (m_CurrentHand == HandSource.LeftHand) ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
    }

    private void AssignObjectToHand(HandSource hand, GameObject go)
    {
        if (hand == HandSource.LeftHand)
            m_CurrentLeftObject = go;
        else
            m_CurrentRightObject = go;
    }

    private HandSource GetOtherHand()
    {
        return (m_CurrentHand == HandSource.LeftHand) ? HandSource.RightHand : HandSource.LeftHand;
    }

    private GameObject GetCurrentHandObject(bool otherHand = false)
    {
        if (m_CurrentHand == HandSource.LeftHand)
            return (otherHand) ? m_CurrentRightObject : m_CurrentLeftObject;
        else
            return (otherHand) ? m_CurrentLeftObject : m_CurrentRightObject;
    }

}
