using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Useful for e.g. Station Button, Sliding doors, etc.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class StationaryObject : MonoBehaviour, IInteractable
{
    public abstract void OnControllerExit();
    public abstract void OnControllerEnter(PlayerViveController.HandSource handSource);
    public abstract void OnControllerStay();
    public abstract void OnUse();
}
