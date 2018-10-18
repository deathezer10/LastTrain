using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorLever : MonoBehaviour
{
  
    private PlayerViveController[] foundControllers;
    private GameObject parent;
    private BoxCollider ButtonCollider;
    private const float m_ToggleOffset = -0.02f;

    // Use this for initialization
    void Start()
    {
        foundControllers = FindObjectsOfType<PlayerViveController>();
        parent = transform.root.gameObject;
        ButtonCollider = GetComponent<BoxCollider>();
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
       
     
    }


    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.name == "Umbrella")
        {

            DriverCabinDoorLock.init();
            transform.DOLocalMoveX(m_ToggleOffset, 0.09f).SetRelative();
        }

    }


}
