﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful for dynamic objects such as balls, fire extinguisher, keycard
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class GrabbableObject : MonoBehaviour, IGrabbable, IInteractable
{
    public abstract void OnControllerExit();
    public abstract void OnControllerEnter(PlayerViveController.HandSource handSource);
    public abstract void OnControllerStay();
    public abstract void OnGrab();
    public abstract void OnGrabReleased(bool snapped);
    public abstract void OnUse();
}
