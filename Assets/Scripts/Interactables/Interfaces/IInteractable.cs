using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable
{
    /// <summary>
    /// Called once when the vive controller touches the collider
    /// </summary>
    /// <param name="handSource">The hand that touched this object</param>
    void OnControllerEnter(PlayerViveController currentController);

    /// <summary>
    /// Called when the vive controller left the collider without grabbing the object
    /// </summary>
    void OnControllerExit();

    /// <summary>
    /// Called once every frame when the vive controller is inside the collider
    /// </summary>
    void OnControllerStay();

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it, called once every frame while held down
    /// </summary>
    void OnUseDown();

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it, called only once
    /// </summary>
    void OnUse();

    /// <summary>
    /// Called when the player pressed the grip button WHILE grabbing it, called once after the player relased the Use button
    /// </summary>
    void OnUseUp();
}
