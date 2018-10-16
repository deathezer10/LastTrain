using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverCabinDoorLock : MonoBehaviour {

    public static bool bIsLocked = true;

   

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void init()
    {
        //Do Sounds? 
        bIsLocked = false;
    } 

    public static bool GetLock()
    {
        return bIsLocked;
    }

}
