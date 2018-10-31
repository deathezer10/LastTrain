using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful for dynamic objects such as balls, fire extinguisher, keycard
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class GrabbableObject : MonoBehaviour, IGrabbable, IInteractable
{
    public virtual bool hideControllerOnGrab { get { return true; } }
    public virtual void OnControllerExit() { }
    public virtual void OnControllerEnter(PlayerViveController currentController) { }
    public virtual void OnControllerStay() { }
    public virtual void OnGrab() { }
    public virtual void OnGrabStay() { }
    public virtual void OnGrabReleased() { }
    public virtual void OnUseDown() { }
    public virtual void OnUse() { }
    public virtual void OnUseUp() { }
}
