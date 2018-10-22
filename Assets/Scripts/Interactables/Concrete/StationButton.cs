using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;

public class StationButton : StationaryObject {

    public GameObject m_Train;
    private const float m_ToggleOffset = -0.03f;
    private bool m_Toggled = false;
    
	void Start ()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    public override void OnControllerEnter(PlayerViveController currentController, PlayerViveController.HandSource handSource)
    {
        if (m_Toggled == false)
        {
            m_Toggled = true;

            SteamVR_Input.actionsVibration[0].Execute(0, 0.2f, 5, 0, currentController.HandSourceToInputSource());

            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            m_Train.GetComponent<TrainArriver>().BeginArrival();

            transform.DOLocalMoveX(m_ToggleOffset, 0.2f).SetRelative();
        }
    }

    public override void OnControllerExit()
    {
    }

    public override void OnControllerStay()
    {
    }

    public override void OnGrab()
    {
    }

    public override void OnGrabReleased()
    {
    }

    public override void OnUse()
    {
    }
}
