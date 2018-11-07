using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour {

    [SerializeField]
    bool m_RotateX = true, m_RotateY = true, m_RotateZ = true;

    private void LateUpdate()
    {
        Vector3 pos = Camera.main.transform.position;

        if (!m_RotateX)
            pos.x = transform.position.x;
        if (!m_RotateY)
            pos.y = transform.position.y;
        if (!m_RotateZ)
            pos.z = transform.position.z;

        transform.LookAt(pos);
    }

}
