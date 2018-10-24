using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful for dynamic objects such as balls, fire extinguisher, keycard
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class GrabbableObject : MonoBehaviour, IGrabbable, IInteractable
{
    public abstract void OnControllerExit();
    public abstract void OnControllerEnter(PlayerViveController currentController);
    public abstract void OnControllerStay();
    public abstract void OnGrab();
    public abstract void OnGrabReleased();
    public abstract void OnUse();
}
