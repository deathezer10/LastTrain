using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCameraControl : MonoBehaviour
{
    public float d_CameraSensitivity;

    Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
    }
    
    void Update()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            originalRotation = transform.localRotation;

            float rotationX = Input.GetAxis("Mouse X") * d_CameraSensitivity;
            float rotationY = Input.GetAxis("Mouse Y") * d_CameraSensitivity;

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            Quaternion finalRotation = originalRotation * xQuaternion * yQuaternion;

            finalRotation.eulerAngles = new Vector3(finalRotation.eulerAngles.x, finalRotation.eulerAngles.y, 0);

            transform.localRotation = finalRotation;
        }
    }
}
