using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Useful for e.g. Station Button, Sliding doors, etc.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class StationaryObject : MonoBehaviour, IInteractable, IStationaryGrabbable
{
    public abstract void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource);
    public abstract void OnControllerExit();
    public abstract void OnControllerStay();
    public abstract void OnGrab();
    public abstract void OnGrabReleased();
    public abstract void OnUse();
}
