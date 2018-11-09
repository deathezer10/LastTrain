using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{

    public Transform m_FollowTarget;
    public bool m_FollowX, m_FollowY, m_FollowZ;

    private void LateUpdate()
    {
        if (m_FollowTarget == null)
            return;

        Vector3 newPos = transform.position;

        if (m_FollowX)
            newPos.x = m_FollowTarget.position.x;
        if (m_FollowY)
            newPos.y = m_FollowTarget.position.y;
        if (m_FollowZ)
            newPos.z = m_FollowTarget.position.z;

        transform.position = newPos;
    }

}
