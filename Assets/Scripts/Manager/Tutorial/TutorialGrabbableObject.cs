using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGrabbableObject : GrabbableObject
{
    [SerializeField]
    private GameObject _markerObj;
    public GameObject MarkerObject { get { return _markerObj; } set { _markerObj = value; } }
}
