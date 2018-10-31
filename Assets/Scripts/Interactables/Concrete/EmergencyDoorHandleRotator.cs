using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoorHandleRotator : StationaryObject
{

    private PlayerViveController m_Controller;

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        m_Controller = currentController;
    }
    
    public override void OnGrabStay()
    {
        if (m_Controller != null)
        {
            
        }
    }


}
