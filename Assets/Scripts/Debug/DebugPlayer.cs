using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugPlayer : MonoBehaviour {

    private CharacterController m_CController;
    private Camera m_CurrentCamera;

    private const float m_PlayerMoveSpeed = 3;

    [SerializeField]
    private UnityEvent m_inputEvent;

    private void Start()
    {
        m_CController = GetComponent<CharacterController>();
        m_CurrentCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        MouseViewUpdate();
        
        if (Input.GetMouseButton(1))
        {
            MouseUpdate();
        }

        if(Input.GetMouseButtonDown(0))
        {
            m_inputEvent.Invoke();
        }

        PlayerUpdate();
    }

    private void PlayerUpdate()
    {
        var moveDir = m_CurrentCamera.transform.forward * m_PlayerMoveSpeed;

        if (Input.GetKey(KeyCode.W)) m_CController.SimpleMove(moveDir);
        if (Input.GetKey(KeyCode.S)) m_CController.SimpleMove(-moveDir);
    }

    private void MouseViewUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void MouseUpdate()
    {
        var horRot = m_CurrentCamera.transform;
        var verRot = horRot.parent;

        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        verRot.transform.Rotate(-Y_Rotation, X_Rotation, 0);
        var rot = verRot.transform.localEulerAngles;
        rot.z = 0.0f;
        verRot.transform.localEulerAngles = rot;
    }
}
