using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class DebugViveController : PlayerViveController {

    override protected void OnTriggerEnter(Collider other)
    {
        if (PlayerOriginHandler.IsOutsideOrigin)
            return;

        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerEnter(this, GetCurrentHand());
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
                // On Grab
                if (Input.GetMouseButtonDown(0))
                {
                    grabbableObject.OnGrab();

                    if (other.GetComponent<IStationaryGrabbable>() == null)
                    {
                        Rigidbody rb = other.GetComponent<Rigidbody>();
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
                    grabbableObject.OnGrabReleased(false);

                    Destroy(GetComponent<FixedJoint>());
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    iObject.OnUse();
                }
            }
        }
    }

    override protected void OnTriggerExit(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            iObject.OnControllerExit();

            var grabbableObject = other.GetComponent<IGrabbable>();

            grabbableObject.OnGrabReleased(true);
            Destroy(GetComponent<FixedJoint>());
        }
    }
}
