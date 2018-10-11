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
    /// Called when the player releases the Trigger button WHILE grabbing it or if the object moves out of the trigger box (snapped)
    /// </summary>
    /// <param name="snapped">Did the object moved out of trigger box while we were holding it?</param>
    void OnGrabReleased(bool snapped);

}
