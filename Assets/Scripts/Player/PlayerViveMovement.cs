using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerViveMovement : MonoBehaviour {

    private CharacterController m_CController;

    private void Start()
    {
        m_CController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 moveDir = Vector3.zero;

        m_CController.SimpleMove(moveDir * Time.deltaTime);
    }

}
