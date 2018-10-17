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

    private Collider m_CurrentObjectCollider;

    private void Update()
    {
        if (m_CurrentObjectCollider != null)
        {
            if (PlayerOriginHandler.IsOutsideOrigin)
                return;

            var iObject = m_CurrentObjectCollider.GetComponent<IInteractable>();

            if (iObject != null)
            {
                iObject.OnControllerStay();

                var grabbableObject = m_CurrentObjectCollider.GetComponent<IGrabbable>();

                if (grabbableObject != null)
                {

                    // On Grab
                    if (GetCurrentHandObject() == null)
                    {
                        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(HandSourceToInputSource()))
                        {
                            // If other hand is holding this object, unassign it
                            if (GetCurrentHandObject(true) != null && GetCurrentHandObject(true) == m_CurrentObjectCollider.gameObject)
                            {
                                AssignObjectToHand(GetOtherHand(), null);
                            }

                            grabbableObject.OnGrab();

                            AssignObjectToHand(m_CurrentHand, m_CurrentObjectCollider.gameObject);

                            if (m_CurrentObjectCollider.GetComponent<IStationaryGrabbable>() == null)
                            {
                                Rigidbody rb = m_CurrentObjectCollider.GetComponent<Rigidbody>();
                                rb.velocity = Vector3.zero;

                                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                                joint.breakForce = 7500;
                                joint.breakTorque = Mathf.Infinity;
                                joint.connectedBody = rb;
                            }
                        }
                    }

                    // On Grab Released
                    if (GetCurrentHandObject() != null)
                    {
                        if (SteamVR_Input._default.inActions.GrabUse.GetStateDown(HandSourceToInputSource()))
                        {
                            iObject.OnUse();
                            Debug.Log("On Use");
                        }

                        if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(HandSourceToInputSource()))
                        {
                            grabbableObject.OnGrabReleased(false);

                            AssignObjectToHand(m_CurrentHand, null);

                            Destroy(GetComponent<FixedJoint>());

                            if (m_CurrentObjectCollider.GetComponent<IStationaryGrabbable>() == null)
                            {
                                Rigidbody rb = m_CurrentObjectCollider.GetComponent<Rigidbody>();
                                rb.velocity = GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
                                rb.angularVelocity = GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();
                            }
                        }
                    }

                }
            }
        }
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        if (PlayerOriginHandler.IsOutsideOrigin)
            return;

        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerEnter(this, m_CurrentHand);
            m_CurrentObjectCollider = other;
        }

    }

    virtual protected void OnTriggerStay(Collider other)
    {
    }

    virtual protected void OnTriggerExit(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerExit();

            var grabbableObject = other.GetComponent<IGrabbable>();

            if (grabbableObject != null && GetCurrentHandObject() == other.gameObject)
            {
                m_CurrentObjectCollider = null;
                grabbableObject.OnGrabReleased(true);
                AssignObjectToHand(m_CurrentHand, null);
                Destroy(GetComponent<FixedJoint>());
            }
        }
    }

    private void OnJointBreak(float breakForce)
    {
        m_CurrentObjectCollider = null;
        AssignObjectToHand(m_CurrentHand, null);
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

    public HandSource GetCurrentHand()
    {
        return m_CurrentHand;
    }

    private GameObject GetCurrentHandObject(bool otherHand = false)
    {
        if (m_CurrentHand == HandSource.LeftHand)
            return (otherHand) ? m_CurrentRightObject : m_CurrentLeftObject;
        else
            return (otherHand) ? m_CurrentLeftObject : m_CurrentRightObject;
    }

    public static PlayerViveController GetControllerThatHolds(GameObject obj)
    {
        var controllers = FindObjectsOfType<PlayerViveController>();

        foreach (PlayerViveController controller in controllers)
        {
            if (controller.GetCurrentHandObject() == obj)
                return controller;
        }

        return null;
    }

}
