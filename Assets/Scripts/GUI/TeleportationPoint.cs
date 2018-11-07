using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportationPoint : MonoBehaviour
{

    [Tag, SerializeField]
    List<string> m_TriggerWhitelist;

    [SerializeField]
    UnityEvent m_OnTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in m_TriggerWhitelist)
        {
            if (tag == other.tag)
            {
                m_OnTriggerEnter?.Invoke();
                return;
            }
        }
    }

}
