using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCircularMesh : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutorialObj;

    private Vector3 _offSet;

    private void Awake()
    {
        _offSet = this.transform.localPosition;
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.localPosition = _tutorialObj.transform.localPosition + _offSet;
    }

}
