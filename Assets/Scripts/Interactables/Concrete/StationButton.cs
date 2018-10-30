using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;

public class StationButton : StationaryObject, IShootable
{

    public GameObject m_Train;
    private const float m_ToggleOffset = -0.03f;
    private bool m_Toggled = false;

    void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        if (m_Toggled == false)
        {
            m_Toggled = true;

            if (currentController != null)
            {
                var source = currentController.GetCurrentHand().ToInputSource();
                currentController.Vibration(0, 0.7f, 10, 1, source);
            }

            GetComponent<AudioPlayer>().Play();

            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            m_Train.GetComponent<TrainArriver>().BeginArrival(() =>
            {
                transform.Find("AnnouncementBeep").GetComponent<AudioPlayer>().Play();
            });

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

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

    public void OnShot(Revolver revolver)
    {
        OnControllerEnter(null);
    }

}
