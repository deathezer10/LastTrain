using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportationPoint : TutorialObject
{

    [Tag, SerializeField]
    List<string> m_TriggerWhitelist;

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in m_TriggerWhitelist)
        {
            if (tag == other.tag)
            {
                base.OnTriggerEnter(other);

                return;
            }
        }
    }
}
