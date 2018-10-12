using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLever : MonoBehaviour
{
  
    private PlayerViveController[] foundControllers;
    private GameObject parent;
    private BoxCollider LeverTip;

    // Use this for initialization
    void Start()
    {
        foundControllers = FindObjectsOfType<PlayerViveController>();
        parent = transform.root.gameObject;
        LeverTip = GetComponent<BoxCollider>();
        
    }
    
    // Update is called once per frame
    void Update()
    {
       
     
    }


    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.name == "Umbrella")
        {
            //TODO activate button animation/add movement
            //TODO Send to door's script for opening door&Sound&extra=?
        }

    }


}
