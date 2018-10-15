using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
    public static GameObject[] LeftDoors;
    public static GameObject[] RightDoors;
    public GameObject DoorLeft1;
    public GameObject DoorRight1;

    public GameObject DoorLeft2;
    public GameObject DoorRight2;

    public GameObject DoorLeft3;
    public GameObject DoorRight3;

    public GameObject DoorLeft4;
    public GameObject DoorRight4;

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

        LeftDoors.SetValue(DoorLeft1, 0);
        LeftDoors.SetValue(DoorLeft2, 1);
        LeftDoors.SetValue(DoorLeft3, 2);
        LeftDoors.SetValue(DoorLeft4, 3);
        RightDoors.SetValue(DoorRight1, 0);
        RightDoors.SetValue(DoorRight2, 1);
        RightDoors.SetValue(DoorRight3, 2);
        RightDoors.SetValue(DoorRight4, 3);
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
