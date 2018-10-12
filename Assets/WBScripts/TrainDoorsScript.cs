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
    }
    
    void Update()
    {
        // Testing placeholder
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenTrainDoors();
        }

        // Testing placeholder
        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseTrainDoors();
        }

        if (opening && doorLeft.transform.position != doorLeftFinalPos)
        {
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, doorLeftFinalPos, 0.4f * Time.deltaTime);
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, doorRightFinalPos, 0.4f * Time.deltaTime);
        }
        else if (opening)
        {
            opening = false;
        }

        if (closing && doorLeft.transform.position != doorLeftOriginalPos)
        {
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, doorLeftOriginalPos, 0.4f * Time.deltaTime);
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, doorRightOriginalPos, 0.4f * Time.deltaTime);
        }
        else if (closing)
        {
            closing = false;
            this.enabled = false;
        }
    }

    public void OpenTrainDoors()
    {
        Debug.Log("Opening Doors");
        opening = true;
    }

    public void CloseTrainDoors()
    {
        Debug.Log("Closing Doors");
        closing = true;
    }
}
