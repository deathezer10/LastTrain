using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TrainBetweenCabinDoors : MonoBehaviour
{
    float offset = -1.1f;

    List<GameObject> betweenDoors = new List<GameObject>();

    BoxCollider betweenDoorsCollider;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;

            if (child.name.Contains("BtwnCabinDr"))
            {
                betweenDoors.Add(child);
            }
        }

        betweenDoorsCollider = gameObject.AddComponent<BoxCollider>();
        betweenDoorsCollider.center = new Vector3(0f, 1.1f, -9.62f);
        betweenDoorsCollider.size = new Vector3(1.25f, 2f, 1.25f);
    }

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
        yield return new WaitForSeconds(1f);

        Debug.Log("Play Announcement");
        // Call for the next announcement

        yield return new WaitForSeconds(1.05f);

        betweenDoorsCollider.enabled = false;

        foreach (GameObject door in betweenDoors)
        {
            door.SetActive(false);
        }
    }
}
