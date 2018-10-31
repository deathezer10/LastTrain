using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Useful for e.g. Station Button, Sliding doors, etc.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class StationaryObject : MonoBehaviour, IInteractable, IStationaryGrabbable
{
    public virtual bool hideControllerOnGrab { get { return true; } }
    public virtual void OnControllerEnter(PlayerViveController currentController) { }
    public virtual void OnControllerExit() { }
    public virtual void OnControllerStay() { }
    public virtual void OnGrab() { }
    public virtual void OnGrabStay() { }
    public virtual void OnGrabReleased() { }
    public virtual void OnUse() { }
    public virtual void OnUseDown() { }
    public virtual void OnUseUp() { }
}