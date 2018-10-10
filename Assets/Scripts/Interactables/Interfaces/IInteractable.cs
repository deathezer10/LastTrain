using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{

    /// <summary>
    /// Called once when the vive controller touches the collider
    /// </summary>
    /// <param name="handSource">The hand that touched this object</param>
    void OnControllerEnter(PlayerViveController.HandSource handSource);

    /// <summary>
    /// Called when the vive controller left the collider without grabbing the object
    /// </summary>
    void OnControllerExit();

    /// <summary>
    /// Called every frame when the vive controller is inside the collider
    /// </summary>
    void OnControllerStay();

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it
    /// </summary>
    void OnUse();
    
}
