using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DummyPlayerMovement : MonoBehaviour
{

    public float d_PlayerMoveSpeed;

    private CharacterController d_CController;
    private Camera d_CurrentCamera;

    private void Start()
    {
        d_CController = GetComponent<CharacterController>();
        d_CurrentCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDir = d_CurrentCamera.transform.forward * d_PlayerMoveSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveDir = -d_CurrentCamera.transform.forward * d_PlayerMoveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir -= d_CurrentCamera.transform.right * d_PlayerMoveSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir += d_CurrentCamera.transform.right * d_PlayerMoveSpeed;
        }

        d_CController.SimpleMove(moveDir);
    }
}
