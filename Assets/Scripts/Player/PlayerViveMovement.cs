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

    private void Start()
    {
        m_CController = GetComponent<CharacterController>();
        m_CurrentCamera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = Vector3.zero;

        if ( SteamVR_Input._default.inActions.Move.GetState(SteamVR_Input_Sources.Any))
        {
            moveDir = m_CurrentCamera.transform.forward * m_PlayerMoveSpeed;
        }

        m_CController.SimpleMove(moveDir);
    }

}
