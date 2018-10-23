using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class DebugViveController : PlayerViveController
{
    private static GameObject m_CurrentDebugObject = null;

    override protected void LateUpdate()
    {
        var currentObject = m_CurrentDebugObject;

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
                    if (Input.GetMouseButtonDown(0))
                    {
                        grabbableObject.OnGrab();

                        if (currentObject.GetComponent<IStationaryGrabbable>() == null)
                        {
                            Rigidbody rb = currentObject.GetComponent<Rigidbody>();
                            rb.velocity = Vector3.zero;

                            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                            joint.breakForce = 7500;
                            joint.breakTorque = Mathf.Infinity;
                            joint.connectedBody = rb;
                        }
                    }

                    // On Grab Released
                    if (Input.GetMouseButtonUp(0))
                    {
                        grabbableObject.OnGrabReleased();

                        Destroy(GetComponent<FixedJoint>());
                    }

                    if (Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        iObject.OnUse();
                    }
                }
            }
        }
    }


    override protected void OnTriggerEnter(Collider other)
    {
        if (PlayerOriginHandler.IsOutsideOrigin)
            return;

        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerEnter(this);
        }
    }

    override protected void OnTriggerStay(Collider other)
    {
        if (PlayerOriginHandler.IsOutsideOrigin)
            return;

        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerStay();

            var grabbableObject = other.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                m_CurrentDebugObject=other.gameObject;
            }
        }
    }

    override protected void OnTriggerExit(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerExit();

            Destroy(GetComponent<FixedJoint>());
        }
    }

    override public void Vibration(
        float secondsFromNow,
        float durationSeconds,
        float frequency,
        float amplitude,
        SteamVR_Input_Sources handSource)
    {

    }
}
