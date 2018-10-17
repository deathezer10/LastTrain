using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{

    /// <summary>
    /// Called when the player grabs the object using Trigger button
    /// </summary>
    void OnGrab();

    /// <summary>
    /// Called when the player releases the Trigger button WHILE grabbing it
    /// </summary>
    void OnGrabReleased();

}
