using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLever : MonoBehaviour
{
  
    private PlayerViveController[] foundControllers;
    private GameObject parent;
    private BoxCollider ButtonCollider;
    

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

            transform.position = transform.TransformPoint(ButtonCollider.bounds.center - (ButtonCollider.bounds.max / 2));
            DriverCabinDoorLock.init();
        
        }

    }


}
