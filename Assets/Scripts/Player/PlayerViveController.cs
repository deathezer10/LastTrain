using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public enum HandSource
{
    LeftHand,
    RightHand
}

public static partial class HandSourceEx
{
    public static SteamVR_Input_Sources ToInputSource(this HandSource param)
    {
        return (param == HandSource.LeftHand) ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
    }
}

[RequireComponent(typeof(Collider))]
public class PlayerViveController : MonoBehaviour
{
    public HandSource m_CurrentHand;

    private static GameObject m_CurrentLeftObject = null;
    private static GameObject m_CurrentRightObject = null;


    virtual protected void LateUpdate()
    {
        var currentObject = GetCurrentHandObject();

        if (currentObject != null)
        {
            if (PlayerOriginHandler.IsOutsideOrigin)
                return;

            var iObject = currentObject.GetComponent<IInteractable>();

            if (iObject != null)
            {
                iObject.OnControllerStay();

                var grabbableObject = currentObject.GetComponent<IGrabbable>();

                if (grabbableObject != null)
                {

                    // On Grab
                    if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(m_CurrentHand.ToInputSource()))
                    {
                        // If other hand is holding this object, unassign it
                        if (GetCurrentHandObject(true) != null && GetCurrentHandObject(true) == currentObject.gameObject)
                        {
                            Destroy(currentObject.GetComponent<FixedJoint>());
                            AssignObjectToHand(GetOtherHand(), null);
                        }

                        grabbableObject.OnGrab();

                        AssignObjectToHand(m_CurrentHand, currentObject.gameObject);

                        if (currentObject.GetComponent<IStationaryGrabbable>() == null)
                        {
                            FixedJoint joint = currentObject.AddComponent<FixedJoint>();
                            joint.breakForce = 20000;
                            joint.breakTorque = Mathf.Infinity;
                            joint.connectedBody = GetComponent<Rigidbody>();

                            currentObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        }
                    }


                    // On Grab Released
                    if (SteamVR_Input._default.inActions.GrabUse.GetStateDown(m_CurrentHand.ToInputSource()))
                    {
                        iObject.OnUse();
                    }

                    if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(m_CurrentHand.ToInputSource()))
                    {
                        grabbableObject.OnGrabReleased();

                        AssignObjectToHand(m_CurrentHand, null);

                        Destroy(currentObject.GetComponent<FixedJoint>());

                        if (currentObject.GetComponent<IStationaryGrabbable>() == null)
                        {
                            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
                            rb.velocity = transform.root.TransformDirection(GetComponent<SteamVR_Behaviour_Pose>().GetVelocity());
                            rb.angularVelocity = transform.root.TransformDirection(GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity());
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
            AssignObjectToHand(GetCurrentHand(), other.gameObject);
            iObject.OnControllerEnter(this);
        }
    }

    virtual protected void OnTriggerStay(Collider other)
    {
        if (PlayerOriginHandler.IsOutsideOrigin)
            return;

        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null && GetCurrentHandObject(true) != other.gameObject)
        {
            AssignObjectToHand(GetCurrentHand(), other.gameObject);
        }
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
                AssignObjectToHand(m_CurrentHand, null);
                Destroy(other.GetComponent<FixedJoint>());
            }
        }
    }

    private void OnJointBreak(float breakForce)
    {
        AssignObjectToHand(m_CurrentHand, null);
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

    virtual public void Vibration(
        float secondsFromNow, 
        float durationSeconds, 
        float frequency, 
        float amplitude,
        SteamVR_Input_Sources handSource)
    {
        SteamVR_Input.actionsVibration[0].Execute(secondsFromNow, durationSeconds, frequency, amplitude, handSource);
    }

    public void VibrationCurrentHand(
        float secondsFromNow,
        float durationSeconds,
        float frequency,
        float amplitude)
    {
        Vibration(secondsFromNow, durationSeconds, frequency, amplitude, m_CurrentHand.ToInputSource());
    }

}
