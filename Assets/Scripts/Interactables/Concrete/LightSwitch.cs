using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;

public class LightSwitch : StationaryObject
{
    public GameObject m_BombContainer;
    List<Light> m_TrainLights = new List<Light>();
    private bool bSwitchIsOn = false;

    private TrainTimeHandler m_TrainTimeHandler;
    private TrainDoorHandler m_TrainDoorHandler;
    private StationMover m_StationMover;

    private AudioPlayer Audio;

    private bool m_FirstTime = true;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            m_TrainLights.Add(transform.GetChild(i).GetComponent<Light>());
            Audio = GetComponent<AudioPlayer>();
        }

        // lol
        m_TrainTimeHandler = FindObjectOfType<TrainTimeHandler>();
        m_TrainDoorHandler = FindObjectOfType<TrainDoorHandler>();
        m_StationMover = FindObjectOfType<StationMover>();
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        Audio.Play();

        var source = currentController.GetCurrentHand().ToInputSource();
        currentController.Vibration(0, 0.2f, 5, 1, source);

        if (bSwitchIsOn)
        {
            bSwitchIsOn = false;

            transform.localRotation = Quaternion.Euler(0, 0, 0);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(false);
            }
        }
        else
        {
            bSwitchIsOn = true;
            
            if (m_FirstTime)
            {
                m_BombContainer.SetActive(true);

                foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = false;
                }

                m_BombContainer.transform.DOLocalMoveY(0.5f, 5).OnComplete(() =>
                {
                    foreach (var collider in m_BombContainer.transform.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = true;
                    }
                });

                m_TrainDoorHandler.ToggleDoors(false, () =>
                {
                    m_StationMover.ToggleMovement(true);
                    m_TrainTimeHandler.StartTrainTime();
                });

                m_FirstTime = false;
            }

            transform.localRotation = Quaternion.Euler(0, 0, -90);

            foreach (Light light in m_TrainLights)
            {
                light.gameObject.SetActive(true);
            }
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
