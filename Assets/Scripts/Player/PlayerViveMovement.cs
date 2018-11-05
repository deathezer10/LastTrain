using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(CharacterController))]
public class PlayerViveMovement : MonoBehaviour
{

    private CharacterController m_CController;
    private Camera m_CurrentCamera;

    private const float m_PlayerMoveSpeed = 2;
    private const float m_ForwardTouchpadThreshold = 0.15f;

    private bool m_MovementEnabled = false;

    private void Start()
    {
        m_CController = GetComponent<CharacterController>();
        m_CurrentCamera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = Vector3.zero;

        SteamVR_Input_Sources currentHand = SteamVR_Input_Sources.Any;

        if (m_MovementEnabled)
        {
            if (SteamVR_Input._default.inActions.Move.GetState(SteamVR_Input_Sources.LeftHand))
            {
                currentHand = SteamVR_Input_Sources.LeftHand;
            }
            else if (SteamVR_Input._default.inActions.Move.GetState(SteamVR_Input_Sources.RightHand))
            {
                currentHand = SteamVR_Input_Sources.RightHand;
            }

            if (currentHand != SteamVR_Input_Sources.Any)
            {
                if (SteamVR_Input._default.inActions.MoveDirectionPad.GetAxis(currentHand).y >= (m_ForwardTouchpadThreshold - 1))
                {
                    moveDir = m_CurrentCamera.transform.forward * m_PlayerMoveSpeed;
                }
                else
                {
                    moveDir = -m_CurrentCamera.transform.forward * (m_PlayerMoveSpeed * 0.2f);
                }
            }
        }

        m_CController.SimpleMove(moveDir);
    }

}
