using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationTileDestroyer : MonoBehaviour {

    GameObject m_Train;
    float m_DestroyDistance = 0;

    public void SetUpDestroyer(GameObject train, float destroyDistance)
    {
        m_Train = train;
        m_DestroyDistance = destroyDistance;
    }

    private void Update()
    {
        if ((transform.position - m_Train.transform.position).magnitude >= m_DestroyDistance)
        {
            Destroy(gameObject);
        }
    }

}