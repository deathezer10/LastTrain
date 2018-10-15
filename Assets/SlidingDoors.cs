using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
    public static GameObject[] LeftDoors;
    public static GameObject[] RightDoors;
    public static GameObject DoorLeft1;
    public static GameObject DoorRight1;

    public static GameObject DoorLeft2;
    public static GameObject DoorRight2;

    public static GameObject DoorLeft3;
    public static GameObject DoorRight3;

    public static GameObject DoorLeft4;
    public static GameObject DoorRight4;

    public static bool bAreDoorsOpen = false;
    public static bool bOpenDoor = false;
    public static bool bCloseDoor = false;
    public static float speed = 0.1f;
    public static float OpenTime = 5.0f;
    public static bool bDoorsMoving = false;
    // Use this for initialization
    void Start()
    {
        DoorLeft1 = transform.gameObject.transform.GetChild(0).gameObject;
        DoorRight1 = transform.gameObject.transform.GetChild(1).gameObject;

        DoorLeft2 = transform.gameObject.transform.GetChild(2).gameObject;
        DoorRight2 = transform.gameObject.transform.GetChild(3).gameObject;

        DoorLeft3 = transform.gameObject.transform.GetChild(4).gameObject;
        DoorRight3 = transform.gameObject.transform.GetChild(5).gameObject;

        DoorLeft4 = transform.gameObject.transform.GetChild(6).gameObject;
        DoorRight4 = transform.gameObject.transform.GetChild(7).gameObject;

        LeftDoors[0] = DoorLeft1;
        LeftDoors[1] = DoorLeft2;
        LeftDoors[2] = DoorLeft3;
        LeftDoors[3] = DoorLeft4;
        RightDoors[0] = DoorRight1;
        RightDoors[1] = DoorRight2;
        RightDoors[2] = DoorRight3;
        RightDoors[3] = DoorRight4;

        
    }

    // Update is called once per frame
    void Update()
    {
       
   
    }


   public static IEnumerator Open()
    {
        bDoorsMoving = true;
        bAreDoorsOpen = true;
        float time = 0;
        var OpenSpeed = Time.deltaTime * speed;


        while (time < OpenTime)
        {
            time += Time.deltaTime;
            foreach(GameObject door in LeftDoors)
            {
                door.transform.position += new Vector3(0,0,OpenSpeed);
            }

            foreach (GameObject door in RightDoors)
            {
                door.transform.position += new Vector3(0, 0, -OpenSpeed);
            }

            yield return null;
        }

        bDoorsMoving = false;
    }

    public static IEnumerator Close()
    {
        bDoorsMoving = true;
        bAreDoorsOpen = true;
        float time = 0;
        var OpenSpeed = Time.deltaTime * speed;


        while (time < OpenTime)
        {
            time += Time.deltaTime;
            foreach (GameObject door in LeftDoors)
            {
                door.transform.position += new Vector3(0, 0, OpenSpeed);
            }

            foreach (GameObject door in RightDoors)
            {
                door.transform.position += new Vector3(0, 0, -OpenSpeed);
            }

            yield return null;
        }

        bDoorsMoving = false;
    }

   public void UseDoors()
    {
        if (!bDoorsMoving)
        {
            if (!bAreDoorsOpen)
            {
                StartCoroutine(Open());
            }

            if (bAreDoorsOpen)
            {
                StartCoroutine(Close());
            }

        }

    }

}
