using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDoorsScript : MonoBehaviour
{
    public GameObject doorLeft, doorRight;
    public Vector3 leftDoorOffset, rightDoorOffset;

    Vector3 doorLeftOriginalPos, doorRightOriginalPos;
    Vector3 doorLeftFinalPos, doorRightFinalPos;
    bool opening, closing;

    void Start()
    {
        doorLeftOriginalPos = doorLeft.transform.position;
        doorLeftFinalPos = doorLeftOriginalPos + leftDoorOffset;

        doorRightOriginalPos = doorRight.transform.position;
        doorRightFinalPos = doorRightOriginalPos + rightDoorOffset;

        opening = true;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Opening Doors");
            opening = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            closing = true;
        }

        if (opening && doorLeft.transform.position != doorLeftFinalPos)
        {
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, doorLeftFinalPos, 0.4f * Time.deltaTime);
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, doorRightFinalPos, 0.4f * Time.deltaTime);
        }
        else
        {
            opening = false;
            closing = true;
        }

        if (closing && doorLeft.transform.position != doorLeftOriginalPos)
        {
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, doorLeftOriginalPos, 0.4f * Time.deltaTime);
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, doorRightOriginalPos, 0.4f * Time.deltaTime);
        }
        else
        {
            closing = false;
        }
    }
}
