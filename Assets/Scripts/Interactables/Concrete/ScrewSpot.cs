using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewSpot : MonoBehaviour
{
    public delegate void TriggerExit(Collider _other);
    public static event TriggerExit OnExit;

    public virtual void OnTriggerExit(Collider other)
    {
        OnExit(other);
    }

    
}
