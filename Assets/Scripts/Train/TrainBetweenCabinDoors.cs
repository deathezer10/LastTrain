using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TrainBetweenCabinDoors : MonoBehaviour
{
    float offset = -1.1f;
    public GameObject[] betweenDoors;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            OpenBetweenDoors();
        }
    }

    public void OpenBetweenDoors()
    {
        foreach (GameObject door in betweenDoors)
        {
            var tweener = door.transform.DOLocalMoveX(offset, 2).SetRelative();
        }

        StartCoroutine(OnDoorOpenFinish());
    }

    IEnumerator OnDoorOpenFinish()
    {
        yield return new WaitForSeconds(2.05f);
        // Call for the next announcement
        Debug.Log("Doors between the cabins are open.");
    }
}
