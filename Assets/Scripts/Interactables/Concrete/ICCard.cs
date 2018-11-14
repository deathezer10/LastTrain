using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICCard : GrabbableObject
{
    private void LateUpdate()
    {
        GetComponent<Negi.Outline>().enabled = true;
    }
}
